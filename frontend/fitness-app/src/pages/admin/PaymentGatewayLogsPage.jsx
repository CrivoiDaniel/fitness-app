import React, { useEffect, useState } from "react";
import axios from "axios";

const PaymentGatewayLogsPage = () => {
    const [logs, setLogs] = useState([]);
    const [take, setTake] = useState(100);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const load = async () => {
        setLoading(true);
        setError("");
        try {
            const authRaw = localStorage.getItem("auth");
            const auth = authRaw ? JSON.parse(authRaw) : null;
            const token = auth?.token;

            const API = "http://localhost:5140";
            const res = await axios.get(`${API}/api/admin/payment-gateway-logs?take=${take}`, {
                headers: token ? { Authorization: `Bearer ${token}` } : {}
            });
            const payload = res.data;

            // suportă: [ ... ]  sau { data: [ ... ] } sau { items: [ ... ] }
            const list =
                Array.isArray(payload) ? payload :
                    Array.isArray(payload?.data) ? payload.data :
                        Array.isArray(payload?.items) ? payload.items :
                            [];

            setLogs(list);
        } catch (e) {
            setError(`HTTP ${e?.response?.status}: ${JSON.stringify(e?.response?.data)}`);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        load();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div className="p-6">
            <div className="flex items-center justify-between gap-4 mb-4">
                <h1 className="text-2xl font-semibold">Payment Gateway Logs</h1>

                <div className="flex items-center gap-2">
                    <label className="text-sm">Rows:</label>
                    <input
                        type="number"
                        className="border rounded px-2 py-1 w-24"
                        value={take}
                        min={1}
                        max={500}
                        onChange={(e) => setTake(Number(e.target.value))}
                    />
                    <button
                        className="border rounded px-3 py-1"
                        onClick={load}
                        disabled={loading}
                    >
                        Refresh
                    </button>
                </div>
            </div>

            {error && <div className="mb-3 text-red-600">{error}</div>}
            {loading && <div className="mb-3">Loading...</div>}

            <div className="overflow-auto border rounded">
                <table className="min-w-[1100px] w-full text-sm">
                    <thead className="bg-gray-100">
                    <tr>
                        <th className="text-left p-2">CreatedAt</th>
                        <th className="text-left p-2">Provider</th>
                        <th className="text-left p-2">SubscriptionId</th>
                        <th className="text-left p-2">Amount</th>
                        <th className="text-left p-2">Currency</th>
                        <th className="text-left p-2">Attempt</th>
                        <th className="text-left p-2">Success</th>
                        <th className="text-left p-2">DurationMs</th>
                        <th className="text-left p-2">TransactionId</th>
                        <th className="text-left p-2">Error</th>
                    </tr>
                    </thead>
                    <tbody>
                    {logs.map((x) => (
                        <tr key={x.id} className="border-t">
                            <td className="p-2">{x.createdAt ? new Date(x.createdAt).toLocaleString() : "-"}</td>
                            <td className="p-2">{x.provider}</td>
                            <td className="p-2">{x.subscriptionId}</td>
                            <td className="p-2">{x.amount}</td>
                            <td className="p-2">{x.currency}</td>
                            <td className="p-2">{x.attempt}</td>
                            <td className="p-2">{x.isSuccess ? "Yes" : "No"}</td>
                            <td className="p-2">{x.durationMs}</td>
                            <td className="p-2">{x.transactionId || "-"}</td>
                            <td className="p-2">{x.errorMessage || "-"}</td>
                        </tr>
                    ))}

                    {logs.length === 0 && !loading && (
                        <tr>
                            <td className="p-3" colSpan={10}>
                                No logs yet.
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default PaymentGatewayLogsPage;