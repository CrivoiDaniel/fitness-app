import React, { useEffect, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { FiRefreshCw, FiTrendingUp, FiTrendingDown, FiDollarSign, FiUsers } from "react-icons/fi";

import { useAuth } from "../../../context/AuthContext";

import {
  getDashboardSummary,
  getTrends,
  getExpiringSubscriptions,
  getCacheInfo,
  refreshStatisticsCache,
} from "../../../api/admin/statistics";

import {
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  Tooltip,
  Legend,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  LineChart,
  Line,
} from "recharts";

function formatMoneyMDL(x) {
  const n = Number(x || 0);
  return `${n.toFixed(2)} MDL`;
}

function formatPercent(x) {
  const n = Number(x || 0);
  return `${n.toFixed(2)}%`;
}

function formatDateTime(iso) {
  if (!iso) return "-";
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return String(iso);
  return d.toLocaleString();
}

const STATUS_COLORS = ["#111827", "#F59E0B", "#6B7280", "#EF4444"]; // active, pending, expired, cancelled

const StatCard = ({ title, value, sub, icon }) => (
  <div className="rounded-2xl border bg-white p-4 shadow-sm">
    <div className="flex items-start justify-between gap-3">
      <div>
        <div className="text-xs uppercase tracking-wide text-gray-500">{title}</div>
        <div className="mt-1 text-2xl font-extrabold text-gray-900">{value}</div>
        {sub ? <div className="mt-1 text-sm text-gray-600">{sub}</div> : null}
      </div>
      <div className="p-2 rounded-xl bg-gray-100 text-gray-700">{icon}</div>
    </div>
  </div>
);

const AdminDashboardPage = () => {
  const { t } = useTranslation();
  const { token } = useAuth();

  const [summary, setSummary] = useState(null);
  const [trends, setTrends] = useState([]);
  const [expiring, setExpiring] = useState([]);
  const [cacheInfo, setCacheInfo] = useState(null);

  const [daysAhead, setDaysAhead] = useState(30);

  const [loading, setLoading] = useState(false);
  const [refreshing, setRefreshing] = useState(false);
  const [error, setError] = useState("");

  const statusPieData = useMemo(() => {
    if (!summary) return [];
    return [
      { name: t("admin.dashboard.status.active"), value: Number(summary.activeSubscriptions || 0) },
      { name: t("admin.dashboard.status.pending"), value: Number(summary.pendingSubscriptions || 0) },
      { name: t("admin.dashboard.status.expired"), value: Number(summary.expiredSubscriptions || 0) },
      { name: t("admin.dashboard.status.cancelled"), value: Number(summary.cancelledSubscriptions || 0) },
    ];
  }, [summary, t]);

  const revenueBars = useMemo(() => {
    const r = summary?.revenueBreakdown;
    if (!r) return [];
    return [
      { name: t("admin.dashboard.revenue.today"), value: Number(r.today || 0) },
      { name: t("admin.dashboard.revenue.week"), value: Number(r.thisWeek || 0) },
      { name: t("admin.dashboard.revenue.month"), value: Number(r.thisMonth || 0) },
      { name: t("admin.dashboard.revenue.year"), value: Number(r.thisYear || 0) },
      { name: t("admin.dashboard.revenue.allTime"), value: Number(r.allTime || 0) },
    ];
  }, [summary, t]);

  const fetchAll = async () => {
    try {
      setLoading(true);
      setError("");

      const [s, tr, exp, ci] = await Promise.all([
        getDashboardSummary(token),
        getTrends(token),
        getExpiringSubscriptions(token, daysAhead),
        getCacheInfo(token),
      ]);

      setSummary(s);
      setTrends(tr || []);
      setExpiring(exp || []);
      setCacheInfo(ci);
    } catch (e) {
      setError(e.message || t("admin.dashboard.errors.load"));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAll();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    // când schimb daysAhead, reîncarcă doar expiring
    (async () => {
      try {
        setError("");
        const exp = await getExpiringSubscriptions(token, daysAhead);
        setExpiring(exp || []);
      } catch (e) {
        setError(e.message || t("admin.dashboard.errors.loadExpiring"));
      }
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [daysAhead]);

  const onRefreshCache = async () => {
    try {
      setRefreshing(true);
      setError("");
      await refreshStatisticsCache(token);
      await fetchAll();
    } catch (e) {
      setError(e.message || t("admin.dashboard.errors.refresh"));
    } finally {
      setRefreshing(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold">{t("admin.dashboard.title")}</h1>
        </div>

        <button
          onClick={fetchAll}
          disabled={loading}
          className="px-4 py-2 rounded border bg-white hover:bg-gray-50 disabled:opacity-50"
        >
          {t("common.refresh")}
        </button>
      </div>

      {error && <div className="p-3 rounded bg-red-50 text-red-700 border">{error}</div>}

      {/* Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
        <StatCard
          title={t("admin.dashboard.cards.totalSubscriptions")}
          value={summary ? summary.totalSubscriptions : "—"}
          sub={summary ? `${t("admin.dashboard.cards.active")}: ${summary.activeSubscriptions}` : ""}
          icon={<FiUsers />}
        />

        <StatCard
          title={t("admin.dashboard.cards.totalRevenue")}
          value={summary ? formatMoneyMDL(summary.totalRevenue) : "—"}
          sub={summary ? `${t("admin.dashboard.cards.monthlyRevenue")}: ${formatMoneyMDL(summary.monthlyRevenue)}` : ""}
          icon={<FiDollarSign />}
        />

        <StatCard
          title={t("admin.dashboard.cards.growth")}
          value={summary ? formatPercent(summary.growthRate) : "—"}
          sub={summary ? `${t("admin.dashboard.cards.churn")}: ${formatPercent(summary.churnRate)}` : ""}
          icon={summary && Number(summary.growthRate) >= 0 ? <FiTrendingUp /> : <FiTrendingDown />}
        />
      </div>

      {/* Charts row */}
      <div className="grid grid-cols-1 xl:grid-cols-3 gap-4">
        {/* Status pie */}
        <div className="rounded-2xl border bg-white p-4 shadow-sm">
          <div className="font-semibold text-gray-900">{t("admin.dashboard.charts.subscriptionsStatus")}</div>
          <div className="text-sm text-gray-600 mt-1">{t("admin.dashboard.charts.subscriptionsStatusHint")}</div>

          <div className="mt-3 h-[280px]">
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie data={statusPieData} dataKey="value" nameKey="name" outerRadius={90} innerRadius={50}>
                  {statusPieData.map((_, idx) => (
                    <Cell key={idx} fill={STATUS_COLORS[idx % STATUS_COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Revenue bars */}
        <div className="rounded-2xl border bg-white p-4 shadow-sm">
          <div className="font-semibold text-gray-900">{t("admin.dashboard.charts.revenueBreakdown")}</div>
          <div className="text-sm text-gray-600 mt-1">{t("admin.dashboard.charts.revenueBreakdownHint")}</div>

          <div className="mt-3 h-[280px]">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={revenueBars}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" tick={{ fontSize: 12 }} />
                <YAxis tick={{ fontSize: 12 }} />
                <Tooltip formatter={(v) => formatMoneyMDL(v)} />
                <Bar dataKey="value" fill="#111827" radius={[8, 8, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Cache */}
        <div className="rounded-2xl border bg-white p-4 shadow-sm">
          <div className="flex items-center justify-between">
            <div>
              <div className="font-semibold text-gray-900">{t("admin.dashboard.cache.title")}</div>
              <div className="text-sm text-gray-600 mt-1">{t("admin.dashboard.cache.hint")}</div>
            </div>

            <button
              onClick={onRefreshCache}
              disabled={refreshing}
              className="px-3 py-2 rounded bg-black text-white font-semibold hover:bg-gray-800 disabled:opacity-50 inline-flex items-center gap-2"
            >
              <FiRefreshCw />
              {t("admin.dashboard.cache.refresh")}
            </button>
          </div>

          <div className="mt-4 grid gap-2 text-sm">
            <div className="flex justify-between">
              <span className="text-gray-600">{t("admin.dashboard.cache.lastUpdate")}</span>
              <span className="font-semibold">{formatDateTime(cacheInfo?.lastUpdate)}</span>
            </div>

            <div className="flex justify-between">
              <span className="text-gray-600">{t("admin.dashboard.cache.expiresAt")}</span>
              <span className="font-semibold">{formatDateTime(cacheInfo?.expiresAt)}</span>
            </div>

            <div className="flex justify-between">
              <span className="text-gray-600">{t("admin.dashboard.cache.isExpired")}</span>
              <span className={cacheInfo?.isExpired ? "font-semibold text-red-600" : "font-semibold text-green-700"}>
                {cacheInfo ? (cacheInfo.isExpired ? t("common.yes") : t("common.no")) : "—"}
              </span>
            </div>

            <div className="flex justify-between">
              <span className="text-gray-600">{t("admin.dashboard.cache.cachedSubscriptions")}</span>
              <span className="font-semibold">{cacheInfo?.cachedSubscriptions ?? "—"}</span>
            </div>

            <div className="flex justify-between">
              <span className="text-gray-600">{t("admin.dashboard.cache.cachedPayments")}</span>
              <span className="font-semibold">{cacheInfo?.cachedPayments ?? "—"}</span>
            </div>
          </div>
        </div>
      </div>

      {/* Trends */}
      <div className="rounded-2xl border bg-white p-4 shadow-sm">
        <div className="font-semibold text-gray-900">{t("admin.dashboard.charts.trends")}</div>
        <div className="text-sm text-gray-600 mt-1">{t("admin.dashboard.charts.trendsHint")}</div>

        <div className="mt-3 h-[320px]">
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={trends}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="period" tick={{ fontSize: 12 }} />
              <YAxis tick={{ fontSize: 12 }} />
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="newSubscriptions" stroke="#111827" strokeWidth={2} name={t("admin.dashboard.trends.new")} />
              <Line type="monotone" dataKey="cancelledSubscriptions" stroke="#EF4444" strokeWidth={2} name={t("admin.dashboard.trends.cancelled")} />
              <Line type="monotone" dataKey="netGrowth" stroke="#10B981" strokeWidth={2} name={t("admin.dashboard.trends.net")} />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Expiring table */}
      <div className="rounded-2xl border bg-white p-4 shadow-sm">
        <div className="flex items-end justify-between gap-3 flex-wrap">
          <div>
            <div className="font-semibold text-gray-900">{t("admin.dashboard.expiring.title")}</div>
            <div className="text-sm text-gray-600 mt-1">{t("admin.dashboard.expiring.hint")}</div>
          </div>

          <div className="flex items-center gap-2">
            <label className="text-sm text-gray-600">{t("admin.dashboard.expiring.daysAhead")}</label>
            <input
              type="number"
              min={1}
              max={365}
              className="w-28 border rounded p-2"
              value={daysAhead}
              onChange={(e) => setDaysAhead(e.target.value)}
            />
          </div>
        </div>

        <div className="mt-4 border rounded-xl overflow-hidden">
          <div className="grid grid-cols-5 gap-2 px-4 py-3 border-b font-semibold text-sm text-gray-700">
            <div>{t("admin.dashboard.expiring.columns.client")}</div>
            <div>{t("admin.dashboard.expiring.columns.plan")}</div>
            <div>{t("admin.dashboard.expiring.columns.endDate")}</div>
            <div>{t("admin.dashboard.expiring.columns.daysRemaining")}</div>
            <div>{t("admin.dashboard.expiring.columns.subscriptionId")}</div>
          </div>

          {loading ? (
            <div className="p-4 text-gray-600">{t("common.loading")}</div>
          ) : (expiring?.length ?? 0) === 0 ? (
            <div className="p-4 text-gray-600">{t("admin.dashboard.expiring.empty")}</div>
          ) : (
            expiring.map((x) => (
              <div key={x.subscriptionId} className="grid grid-cols-5 gap-2 px-4 py-3 border-b text-sm items-center">
                <div className="truncate">{x.clientName}</div>
                <div>{x.planType}</div>
                <div>{new Date(x.endDate).toLocaleDateString()}</div>
                <div className={x.daysRemaining <= 7 ? "font-semibold text-red-700" : "text-gray-700"}>
                  {x.daysRemaining}
                </div>
                <div>#{x.subscriptionId}</div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default AdminDashboardPage;