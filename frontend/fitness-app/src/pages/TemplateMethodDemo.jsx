import React, { useState } from 'react';
import designPatternsApi from '../api/lab/designPatternsApi';
import { useAuth } from '../context/AuthContext';

const TemplateMethodDemo = () => {
    const { user } = useAuth();
    const [report, setReport] = useState(null);
    const [loading, setLoading] = useState(false);

    const generateReport = async (type) => {
        setLoading(true);
        try {
            const data = await designPatternsApi.templateMethod.generateReport(type, user?.id || 1);
            setReport(data);
        } catch (error) {
            console.error("Error generating report", error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="p-8 max-w-5xl mx-auto">
            <div className="mb-10">
                <h1 className="text-3xl font-bold text-slate-900">Report Generator</h1>
                <p className="text-slate-500">Folosim <b>Template Method</b> pentru a genera rapoarte cu structură fixă, dar conținut variabil.</p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-12">
                {/* Card Weight Loss */}
                <div className="bg-white p-8 rounded-3xl border border-slate-100 shadow-sm hover:shadow-xl transition-all group cursor-pointer"
                     onClick={() => generateReport('weightloss')}>
                    <div className="w-16 h-16 bg-rose-50 text-rose-500 rounded-2xl flex items-center justify-center text-3xl mb-6 group-hover:scale-110 transition-transform">
                        <i className="fas fa-weight"></i>
                    </div>
                    <h3 className="text-xl font-bold text-slate-800 mb-2">Weight Loss Progress</h3>
                    <p className="text-slate-500 text-sm mb-6">Analizează evoluția greutății, deficitul caloric și oferă recomandări de nutriție.</p>
                    <button className="w-full py-3 bg-slate-900 text-white font-bold rounded-xl group-hover:bg-rose-600 transition-colors">
                        Generate Weight Report
                    </button>
                </div>

                {/* Card Strength */}
                <div className="bg-white p-8 rounded-3xl border border-slate-100 shadow-sm hover:shadow-xl transition-all group cursor-pointer"
                     onClick={() => generateReport('strength')}>
                    <div className="w-16 h-16 bg-indigo-50 text-indigo-500 rounded-2xl flex items-center justify-center text-3xl mb-6 group-hover:scale-110 transition-transform">
                        <i className="fas fa-dumbbell"></i>
                    </div>
                    <h3 className="text-xl font-bold text-slate-800 mb-2">Strength & Performance</h3>
                    <p className="text-slate-500 text-sm mb-6">Analizează recordurile de forță, volumul de antrenament și oferă sfaturi tehnice.</p>
                    <button className="w-full py-3 bg-slate-900 text-white font-bold rounded-xl group-hover:bg-indigo-600 transition-colors">
                        Generate Strength Report
                    </button>
                </div>
            </div>

            {/* Rezultat Raport */}
            {loading && (
                <div className="flex justify-center py-20">
                    <div className="w-10 h-10 border-4 border-slate-200 border-t-slate-900 rounded-full animate-spin"></div>
                </div>
            )}

            {report && !loading && (
                <div className="bg-white rounded-3xl border border-slate-200 shadow-2xl overflow-hidden animate-in fade-in slide-in-from-bottom-4 duration-500">
                    <div className="bg-slate-900 p-8 text-white">
                        <div className="flex justify-between items-start">
                            <div>
                                <h2 className="text-2xl font-bold mb-1">{report.formattedTitle}</h2>
                                <p className="text-slate-400 text-sm">Generat la: {new Date(report.generatedAt).toLocaleString()}</p>
                            </div>
                            <div className="bg-white/10 px-4 py-2 rounded-lg text-xs font-mono">
                                REF: SUB-{report.clientId}-{Math.floor(Math.random()*1000)}
                            </div>
                        </div>
                    </div>

                    <div className="p-8 grid grid-cols-1 md:grid-cols-3 gap-8">
                        {/* Statistici */}
                        <div className="md:col-span-2">
                            <h4 className="font-bold text-slate-800 mb-4 uppercase tracking-wider text-xs">Analiza Metricilor</h4>
                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                                {Object.entries(report.statistics).map(([key, value]) => (
                                    <div key={key} className="bg-slate-50 p-4 rounded-2xl border border-slate-100">
                                        <p className="text-slate-500 text-xs mb-1">{key}</p>
                                        <p className="text-xl font-bold text-slate-900">{value}</p>
                                    </div>
                                ))}
                            </div>
                            
                            <div className="mt-8 p-4 bg-indigo-50/50 rounded-2xl border border-indigo-100 italic text-slate-600 text-sm">
                                <i className="fas fa-quote-left text-indigo-200 mr-2"></i>
                                {report.data}
                            </div>
                        </div>

                        {/* Recomandări */}
                        <div className="bg-slate-50/50 p-6 rounded-3xl border border-slate-100">
                            <h4 className="font-bold text-slate-800 mb-4 uppercase tracking-wider text-xs">Recomandările Antrenorului</h4>
                            <ul className="space-y-3">
                                {report.recommendations.map((rec, i) => (
                                    <li key={i} className="flex gap-3 text-sm text-slate-700">
                                        <i className="fas fa-check-circle text-emerald-500 mt-1"></i>
                                        {rec}
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TemplateMethodDemo;
