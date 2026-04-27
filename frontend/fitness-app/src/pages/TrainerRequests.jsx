import React, { useState, useEffect } from 'react';
import designPatternsApi from '../api/lab/designPatternsApi';
import { useAuth } from '../context/AuthContext';

const TrainerRequests = () => {
    const { token } = useAuth();
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchRequests();
    }, [token]);

    const fetchRequests = async () => {
        setLoading(true);
        try {
            const data = await designPatternsApi.chainOfResponsibility.getMyRequests(token);
            setRequests(data);
        } catch (error) {
            console.error("Error fetching requests", error);
        } finally {
            setLoading(false);
        }
    };

    const handleRespond = async (requestId, accept) => {
        try {
            await designPatternsApi.chainOfResponsibility.respondToRequest(requestId, accept, token);
            fetchRequests();
        } catch (error) {
            console.error("Error responding to request", error);
        }
    };

    return (
        <div className="p-8 max-w-4xl mx-auto">
            <h1 className="text-3xl font-bold text-slate-900 mb-8">Client Assignment Requests</h1>
            
            {loading ? (
                <div className="flex justify-center py-20">
                    <div className="w-10 h-10 border-4 border-indigo-200 border-t-indigo-600 rounded-full animate-spin"></div>
                </div>
            ) : requests.length === 0 ? (
                <div className="bg-white p-12 rounded-3xl text-center border border-slate-100">
                    <p className="text-slate-500 text-lg">No pending requests at the moment.</p>
                </div>
            ) : (
                <div className="space-y-4">
                    {requests.map((req) => (
                        <div key={req.id} className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100 flex items-center justify-between">
                            <div>
                                <h3 className="font-bold text-slate-800 text-lg">{req.clientName}</h3>
                                <p className="text-slate-500 text-sm italic">"{req.message || 'No message provided'}"</p>
                                <p className="text-xs text-slate-400 mt-2">
                                    Received: {new Date(req.createdAt).toLocaleDateString()}
                                </p>
                            </div>
                            <div className="flex gap-2">
                                <button 
                                    onClick={() => handleRespond(req.id, true)}
                                    className="px-6 py-2 bg-emerald-500 hover:bg-emerald-600 text-white font-bold rounded-xl transition-colors"
                                >
                                    Accept
                                </button>
                                <button 
                                    onClick={() => handleRespond(req.id, false)}
                                    className="px-6 py-2 bg-rose-50 hover:bg-rose-100 text-rose-600 font-bold rounded-xl transition-colors"
                                >
                                    Refuse
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default TrainerRequests;
