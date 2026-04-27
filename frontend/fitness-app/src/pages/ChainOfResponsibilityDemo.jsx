import React, { useState, useEffect } from 'react';
import designPatternsApi from '../api/lab/designPatternsApi';
import { useAuth } from '../context/AuthContext';

const ChainOfResponsibilityDemo = () => {
    const { token } = useAuth();
    const [request, setRequest] = useState({
        preferredSpecialization: '',
        minYearsOfExperience: 0,
        minRating: 0
    });
    const [results, setResults] = useState(null);
    const [loading, setLoading] = useState(false);
    const [demoInfo, setDemoInfo] = useState(null);
    
    // Modal & Toast states
    const [selectedTrainer, setSelectedTrainer] = useState(null);
    const [showProfile, setShowProfile] = useState(false);
    const [assignStatus, setAssignStatus] = useState(null);

    useEffect(() => {
        const fetchInfo = async () => {
            try {
                const response = await designPatternsApi.chainOfResponsibility.getDemoInfo(token);
                setDemoInfo(response);
            } catch (error) {
                console.error("Error fetching demo info", error);
            }
        };
        fetchInfo();
    }, [token]);

    const handleSearch = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            const response = await designPatternsApi.chainOfResponsibility.assignTrainer(request, token);
            const normalized = {
                foundCount: response.foundCount ?? response.FoundCount ?? 0,
                results: (response.results || response.Results || []).map(t => ({
                    id: t.id ?? t.Id,
                    fullName: t.fullName ?? t.FullName,
                    specialization: t.specialization ?? t.Specialization,
                    yearsOfExperience: t.yearsOfExperience ?? t.YearsOfExperience,
                    rating: t.rating ?? t.Rating
                }))
            };
            setResults(normalized);
        } catch (error) {
            console.error("Error assigning trainer", error);
        } finally {
            setLoading(false);
        }
    };

    const handleAssign = async (trainerId) => {
        try {
            await designPatternsApi.chainOfResponsibility.requestTrainer(trainerId, "I would like to start training with you!", token);
            setAssignStatus({ type: 'success', message: 'Assignment request sent successfully!' });
            setTimeout(() => setAssignStatus(null), 3000);
        } catch (error) {
            setAssignStatus({ type: 'error', message: 'Failed to send request.' });
            setTimeout(() => setAssignStatus(null), 3000);
        }
    };

    const openProfile = (trainer) => {
        setSelectedTrainer(trainer);
        setShowProfile(true);
    };

    return (
        <div className="p-8 max-w-6xl mx-auto min-h-screen bg-slate-50">
            {/* Header section with glassmorphism effect */}
            <div className="mb-12 text-center">
                <h1 className="text-4xl font-extrabold text-slate-900 mb-4 tracking-tight">
                    Design Pattern: <span className="text-indigo-600">Chain of Responsibility</span>
                </h1>
                <p className="text-lg text-slate-600 max-w-2xl mx-auto leading-relaxed">
                    Laborator 7 - Behavioral Patterns. Demonstrating a flexible trainer assignment system 
                    using a chain of filters.
                </p>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                {/* Left: Controls */}
                <div className="lg:col-span-1">
                    <div className="bg-white rounded-2xl shadow-xl p-8 border border-slate-100 sticky top-8">
                        <h2 className="text-xl font-bold text-slate-800 mb-6 flex items-center">
                            <span className="w-8 h-8 bg-indigo-100 text-indigo-600 rounded-lg flex items-center justify-center mr-3">
                                <i className="fas fa-sliders-h"></i>
                            </span>
                            Assignment Criteria
                        </h2>
                        
                        <form onSubmit={handleSearch} className="space-y-6">
                            <div>
                                <label className="block text-sm font-medium text-slate-700 mb-2">Specialization</label>
                                <input 
                                    type="text" 
                                    className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all outline-none"
                                    placeholder="e.g. Yoga, Fitness, Bodybuilding"
                                    value={request.preferredSpecialization}
                                    onChange={(e) => setRequest({...request, preferredSpecialization: e.target.value})}
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-slate-700 mb-2">Min. Experience (Years)</label>
                                <input 
                                    type="number" 
                                    className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all outline-none"
                                    value={request.minYearsOfExperience}
                                    onChange={(e) => setRequest({...request, minYearsOfExperience: parseInt(e.target.value) || 0})}
                                />
                            </div>

                            <div>
                                <label className="block text-sm font-medium text-slate-700 mb-2">Min. Rating (0-5)</label>
                                <input 
                                    type="number" 
                                    step="0.1"
                                    max="5"
                                    className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all outline-none"
                                    value={request.minRating}
                                    onChange={(e) => setRequest({...request, minRating: parseFloat(e.target.value) || 0})}
                                />
                            </div>

                            <button 
                                type="submit"
                                disabled={loading}
                                className="w-full py-4 bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-xl shadow-lg shadow-indigo-200 transition-all transform active:scale-95 disabled:opacity-50"
                            >
                                {loading ? "Processing Chain..." : "Find Best Match"}
                            </button>
                        </form>

                        {demoInfo && (
                            <div className="mt-8 pt-8 border-t border-slate-100">
                                <h3 className="text-sm font-bold text-slate-400 uppercase tracking-widest mb-4">Chain Logic</h3>
                                <div className="space-y-3">
                                    {demoInfo.handlers.map((handler, idx) => (
                                        <div key={idx} className="flex items-center text-sm text-slate-600">
                                            <div className="w-1.5 h-1.5 bg-indigo-500 rounded-full mr-3"></div>
                                            {handler}
                                            {idx < demoInfo.handlers.length - 1 && (
                                                <i className="fas fa-chevron-down mx-2 text-slate-300 text-xs"></i>
                                            )}
                                        </div>
                                    ))}
                                </div>
                            </div>
                        )}
                    </div>
                </div>

                {/* Right: Results */}
                <div className="lg:col-span-2">
                    <div className="bg-white rounded-2xl shadow-xl p-8 border border-slate-100 min-h-[500px]">
                        <div className="flex justify-between items-center mb-8">
                            <h2 className="text-2xl font-bold text-slate-800">Results</h2>
                            {results && (
                                <span className="px-4 py-1.5 bg-green-100 text-green-700 rounded-full text-sm font-bold">
                                    {results.foundCount} Trainers Found
                                </span>
                            )}
                        </div>

                        {!results && !loading && (
                            <div className="flex flex-col items-center justify-center h-full text-slate-400 py-20">
                                <div className="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mb-6">
                                    <i className="fas fa-search text-3xl"></i>
                                </div>
                                <p className="text-lg">Set your criteria and run the chain</p>
                            </div>
                        )}

                        {loading && (
                            <div className="flex flex-col items-center justify-center py-20">
                                <div className="w-12 h-12 border-4 border-indigo-200 border-t-indigo-600 rounded-full animate-spin"></div>
                                <p className="mt-4 text-slate-500 font-medium">Executing pattern logic...</p>
                            </div>
                        )}

                        {results && (
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                {(results.results || results.Results)?.map((trainer) => (
                                    <div key={trainer.id} className="group bg-slate-50 hover:bg-white p-6 rounded-2xl border border-transparent hover:border-indigo-100 hover:shadow-xl hover:shadow-indigo-50 transition-all duration-300">
                                        <div className="flex items-start justify-between mb-4">
                                            <div>
                                                <h3 className="font-bold text-slate-900 text-lg group-hover:text-indigo-600 transition-colors">
                                                    {trainer.fullName}
                                                </h3>
                                                <span className="text-sm font-medium text-indigo-500 bg-indigo-50 px-3 py-1 rounded-lg">
                                                    {trainer.specialization}
                                                </span>
                                            </div>
                                            <div className="flex items-center bg-yellow-100 text-yellow-700 px-2 py-1 rounded-lg text-sm font-bold">
                                                <i className="fas fa-star mr-1"></i> {trainer.rating}
                                            </div>
                                        </div>
                                        <div className="flex items-center text-slate-500 text-sm">
                                            <i className="fas fa-calendar-alt mr-2"></i>
                                            {trainer.yearsOfExperience} years of experience
                                        </div>
                                        <div className="mt-6 flex gap-3">
                                            <button 
                                                onClick={() => openProfile(trainer)}
                                                className="flex-1 py-2 bg-white border border-slate-200 hover:border-indigo-500 hover:text-indigo-600 rounded-lg text-sm font-bold transition-all"
                                            >
                                                Profile
                                            </button>
                                            <button 
                                                onClick={() => handleAssign(trainer.id)}
                                                className="flex-1 py-2 bg-indigo-50 text-indigo-600 hover:bg-indigo-600 hover:text-white rounded-lg text-sm font-bold transition-all"
                                            >
                                                Assign
                                            </button>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}

                        {results && results.foundCount === 0 && (
                            <div className="text-center py-20 text-slate-500">
                                <i className="fas fa-exclamation-circle text-4xl mb-4 text-slate-300"></i>
                                <p>No trainers matched the current chain criteria.</p>
                                <button 
                                    onClick={() => setRequest({preferredSpecialization: '', minYearsOfExperience: 0, minRating: 0})}
                                    className="mt-4 text-indigo-600 hover:underline font-bold"
                                >
                                    Reset Filters
                                </button>
                            </div>
                        )}
                    </div>
                </div>
            </div>

            {/* Profile Modal */}
            {showProfile && selectedTrainer && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm">
                    <div className="bg-white w-full max-w-lg rounded-3xl overflow-hidden shadow-2xl animate-in fade-in zoom-in duration-200">
                        <div className="h-32 bg-indigo-600 flex items-end px-8 pb-4">
                            <div className="w-20 h-20 bg-white rounded-2xl shadow-lg flex items-center justify-center text-3xl font-bold text-indigo-600 translate-y-10">
                                {selectedTrainer.fullName?.[0]}
                            </div>
                        </div>
                        <div className="pt-14 px-8 pb-8">
                            <div className="flex justify-between items-start mb-6">
                                <div>
                                    <h2 className="text-2xl font-bold text-slate-800">{selectedTrainer.fullName}</h2>
                                    <p className="text-indigo-600 font-semibold">{selectedTrainer.specialization} Specialist</p>
                                </div>
                                <div className="bg-amber-50 text-amber-600 px-3 py-1 rounded-full text-sm font-bold flex items-center gap-1">
                                    ★ {selectedTrainer.rating}
                                </div>
                            </div>
                            
                            <div className="grid grid-cols-2 gap-4 mb-8">
                                <div className="bg-slate-50 p-4 rounded-2xl">
                                    <p className="text-xs text-slate-400 uppercase mb-1">Experience</p>
                                    <p className="font-bold text-slate-700">{selectedTrainer.yearsOfExperience} Years</p>
                                </div>
                                <div className="bg-slate-50 p-4 rounded-2xl">
                                    <p className="text-xs text-slate-400 uppercase mb-1">Location</p>
                                    <p className="font-bold text-slate-700">Main Center</p>
                                </div>
                            </div>

                            <p className="text-slate-600 text-sm leading-relaxed mb-8">
                                Experienced trainer specializing in {selectedTrainer.specialization}. Dedicated to helping clients achieve their fitness goals through personalized plans and expert guidance.
                            </p>

                            <div className="flex gap-3">
                                <button 
                                    onClick={() => { handleAssign(selectedTrainer.id); setShowProfile(false); }}
                                    className="flex-1 bg-indigo-600 text-white py-3 rounded-2xl font-bold hover:bg-indigo-700 transition-colors"
                                >
                                    Confirm Assignment
                                </button>
                                <button 
                                    onClick={() => setShowProfile(false)}
                                    className="px-6 py-3 border border-slate-200 text-slate-600 rounded-2xl font-bold hover:bg-slate-50 transition-colors"
                                >
                                    Close
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            {/* Assignment Status Toast */}
            {assignStatus && (
                <div className={`fixed bottom-8 right-8 z-50 px-6 py-4 rounded-2xl shadow-lg animate-in slide-in-from-right duration-300 flex items-center gap-3 ${
                    assignStatus.type === 'success' ? 'bg-emerald-500 text-white' : 'bg-rose-500 text-white'
                }`}>
                    <div className="w-6 h-6 rounded-full bg-white/20 flex items-center justify-center">
                        {assignStatus.type === 'success' ? '✓' : '!'}
                    </div>
                    <p className="font-semibold">{assignStatus.message}</p>
                </div>
            )}
        </div>
    );
};

export default ChainOfResponsibilityDemo;
