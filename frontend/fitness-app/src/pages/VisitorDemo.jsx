import React, { useState } from 'react';
import designPatternsApi from '../api/lab/designPatternsApi';

const VisitorDemo = () => {
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(false);

    const runAnalysis = async () => {
        setLoading(true);
        try {
            const data = await designPatternsApi.visitor.runAnalysis();
            setResults(data);
        } catch (error) {
            console.error("Error running analysis", error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="p-8 max-w-6xl mx-auto">
            <div className="mb-10 text-center">
                <h1 className="text-4xl font-bold text-slate-900 mb-4">Analytics Visitor</h1>
                <p className="text-slate-500 max-w-2xl mx-auto">
                    Folosim paternul <b>Visitor</b> pentru a aplica algoritmi de scoring pe entități diferite (Client, Trainer, Plan) fără a le modifica structura internă.
                </p>
                <button 
                    onClick={runAnalysis}
                    className="mt-8 px-10 py-4 bg-indigo-600 text-white font-bold rounded-2xl hover:bg-indigo-700 transition-all shadow-xl shadow-indigo-200 active:scale-95"
                >
                    {loading ? "Analizând datele..." : "Rulează Analiza de Performanță"}
                </button>
            </div>

            {results.length > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-3 gap-8 animate-in fade-in slide-in-from-bottom-6 duration-700">
                    {results.map((res, index) => (
                        <div key={index} className="bg-white rounded-[2rem] border border-slate-100 shadow-sm overflow-hidden flex flex-col">
                            <div className="p-6 flex-1">
                                <div className="flex justify-between items-start mb-6">
                                    <span className="px-3 py-1 bg-slate-100 text-slate-600 rounded-full text-[10px] font-bold uppercase tracking-widest">
                                        {res.category}
                                    </span>
                                    <div className="w-12 h-12 rounded-2xl bg-indigo-50 flex items-center justify-center text-indigo-600">
                                        <i className={`fas ${res.entityName.includes('Client') ? 'fa-user' : res.entityName.includes('Trainer') ? 'fa-user-tie' : 'fa-clipboard-list'}`}></i>
                                    </div>
                                </div>
                                
                                <h3 className="text-xl font-bold text-slate-800 mb-2">{res.entityName}</h3>
                                <p className="text-slate-500 text-sm leading-relaxed mb-6">
                                    {res.analysis}
                                </p>
                            </div>

                            <div className="bg-slate-50 p-6 border-t border-slate-100">
                                <div className="flex items-center justify-between">
                                    <div>
                                        <p className="text-[10px] font-bold text-slate-400 uppercase">Performance Score</p>
                                        <p className="text-3xl font-black text-slate-900">{res.score}<span className="text-sm font-normal text-slate-400">/100</span></p>
                                    </div>
                                    <div className="w-16 h-16 relative">
                                        <svg className="w-full h-full" viewBox="0 0 36 36">
                                            <path
                                                className="text-slate-200"
                                                strokeDasharray="100, 100"
                                                d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831"
                                                fill="none"
                                                stroke="currentColor"
                                                strokeWidth="3"
                                            />
                                            <path
                                                className="text-indigo-600"
                                                strokeDasharray={`${res.score}, 100`}
                                                d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831"
                                                fill="none"
                                                stroke="currentColor"
                                                strokeWidth="3"
                                                strokeLinecap="round"
                                            />
                                        </svg>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {!loading && results.length === 0 && (
                <div className="text-center py-20 bg-slate-50 rounded-[3rem] border-2 border-dashed border-slate-200">
                    <i className="fas fa-microscope text-5xl text-slate-200 mb-4"></i>
                    <p className="text-slate-400">Nicio analiză efectuată încă.</p>
                </div>
            )}

            <div className="mt-12 bg-indigo-900 text-indigo-100 p-8 rounded-[2rem] shadow-2xl relative overflow-hidden">
                <div className="relative z-10">
                    <h4 className="text-xl font-bold mb-4 flex items-center gap-2">
                        <i className="fas fa-lightbulb text-amber-400"></i>
                        Explicarea Paternului Visitor
                    </h4>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-8 text-sm leading-relaxed">
                        <p>
                            Observă cum algoritmul de calcul al scorului nu se află în clasele <code>Client</code> sau <code>Trainer</code>. 
                            Acestea doar "acceptă" vizitatorul. Această abordare ne permite să adăugăm noi operații (ex: un vizitator pentru GDPR) 
                            fără să modificăm niciodată codul sursă al entităților din Domain.
                        </p>
                        <ul className="space-y-2">
                            <li className="flex gap-2">
                                <span className="text-amber-400 font-bold">1.</span>
                                <span><b>Double Dispatch:</b> Elementul redirecționează apelul către metoda corectă a Vizitatorului.</span>
                            </li>
                            <li className="flex gap-2">
                                <span className="text-amber-400 font-bold">2.</span>
                                <span><b>Extensibilitate:</b> Putem crea <code>SecurityVisitor</code> sau <code>AuditVisitor</code> oricând.</span>
                            </li>
                        </ul>
                    </div>
                </div>
                {/* Decorative element */}
                <div className="absolute -right-20 -bottom-20 w-64 h-64 bg-white/5 rounded-full blur-3xl"></div>
            </div>
        </div>
    );
};

export default VisitorDemo;
