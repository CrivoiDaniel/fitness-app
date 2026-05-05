import React, { useState, useEffect } from 'react';
import designPatternsApi from '../api/lab/designPatternsApi';
import { useAuth } from '../context/AuthContext';

const TrainerRequests = () => {
    const { token } = useAuth();
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [selectedRequest, setSelectedRequest] = useState(null);
    const [rejectionReason, setRejectionReason] = useState("");
    const [notification, setNotification] = useState(null);

    useEffect(() => {
        fetchRequests();
    }, [token]);

    const showNotification = (message, type = 'success') => {
        setNotification({ message, type });
        setTimeout(() => setNotification(null), 3000);
    };

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

    const handleMarkReview = async (requestId) => {
        try {
            await designPatternsApi.chainOfResponsibility.markAsUnderReview(requestId, token);
            fetchRequests(); // Refresh for state change
        } catch (error) {
            console.error("Error marking as review", error);
        }
    };

    const handleRespond = async (requestId, accept) => {
        if (!accept && !rejectionReason) {
            showNotification("Please provide a reason for rejection.", "error");
            return;
        }

        try {
            await designPatternsApi.chainOfResponsibility.respondToRequest(requestId, accept, rejectionReason, token);
            showNotification(accept ? "Request accepted!" : "Request rejected.");
            setSelectedRequest(null);
            setRejectionReason("");
            fetchRequests();
        } catch (error) {
            showNotification("Action failed. Check transitions rules.", "error");
            console.error("Error responding to request", error);
        }
    };

    const getStatusBadge = (status) => {
        const styles = {
            Submitted: "bg-blue-100 text-blue-700",
            UnderReview: "bg-amber-100 text-amber-700",
            Accepted: "bg-emerald-100 text-emerald-700",
            Rejected: "bg-rose-100 text-rose-700",
            Cancelled: "bg-slate-100 text-slate-700"
        };
        return (
            <span className={`px-3 py-1 rounded-full text-xs font-bold ${styles[status] || "bg-gray-100"}`}>
                {status}
            </span>
        );
    };

    return (
        <div className="p-8 max-w-5xl mx-auto">
            {notification && (
                <div className={`fixed top-4 right-4 px-6 py-3 rounded-xl shadow-lg text-white font-bold z-50 transition-all ${notification.type === 'error' ? 'bg-rose-500' : 'bg-emerald-500'}`}>
                    {notification.message}
                </div>
            )}
            <div className="flex justify-between items-center mb-8">
                <div>
                    <h1 className="text-3xl font-bold text-slate-900">Client Requests</h1>
                    <p className="text-slate-500 mt-1">Manage your trainer-client connection workflow (State Pattern)</p>
                </div>
                <button onClick={fetchRequests} className="p-2 hover:bg-slate-100 rounded-full transition-colors">
                    <i className="fas fa-sync-alt text-slate-400"></i>
                </button>
            </div>
            
            {loading ? (
                <div className="flex justify-center py-20">
                    <div className="w-10 h-10 border-4 border-indigo-200 border-t-indigo-600 rounded-full animate-spin"></div>
                </div>
            ) : requests.length === 0 ? (
                <div className="bg-white p-12 rounded-3xl text-center border border-slate-100 shadow-sm">
                    <div className="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mx-auto mb-4">
                        <i className="fas fa-inbox text-slate-300 text-3xl"></i>
                    </div>
                    <p className="text-slate-500 text-lg">No connection requests at the moment.</p>
                </div>
            ) : (
                <div className="grid grid-cols-1 gap-4">
                    {requests.map((req) => (
                        <div key={req.id} className="bg-white p-6 rounded-3xl shadow-sm border border-slate-100 hover:shadow-md transition-shadow">
                            <div className="flex items-center justify-between">
                                <div className="flex items-center gap-4">
                                    <div className="w-12 h-12 bg-indigo-100 text-indigo-600 rounded-full flex items-center justify-center font-bold text-xl">
                                        {req.clientName[0]}
                                    </div>
                                    <div>
                                        <div className="flex items-center gap-3">
                                            <h3 className="font-bold text-slate-800 text-lg">{req.clientName}</h3>
                                            {getStatusBadge(req.status)}
                                        </div>
                                        <p className="text-slate-500 text-sm mt-1 italic">"{req.message || 'Hello, I want to train with you!'}"</p>
                                    </div>
                                </div>
                                
                                <div className="flex gap-2">
                                    {req.status === 'Submitted' && (
                                        <button 
                                            onClick={() => handleMarkReview(req.id)}
                                            className="px-4 py-2 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-xl transition-colors text-sm"
                                        >
                                            View / Review
                                        </button>
                                    )}
                                    
                                    {(req.status === 'Submitted' || req.status === 'UnderReview' || req.status === 'Pending') && (
                                        <>
                                            <button 
                                                onClick={() => handleRespond(req.id, true)}
                                                className="px-4 py-2 bg-emerald-500 hover:bg-emerald-600 text-white font-bold rounded-xl transition-colors text-sm"
                                            >
                                                Accept
                                            </button>
                                            <button 
                                                onClick={() => setSelectedRequest(req)}
                                                className="px-4 py-2 bg-rose-50 hover:bg-rose-100 text-rose-600 font-bold rounded-xl transition-colors text-sm"
                                            >
                                                Reject
                                            </button>
                                        </>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* Rejection Modal */}
            {selectedRequest && (
                <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white rounded-3xl p-8 max-w-md w-full shadow-2xl animate-in fade-in zoom-in duration-300">
                        <h2 className="text-2xl font-bold text-slate-900 mb-2">Reject Request</h2>
                        <p className="text-slate-500 mb-6">Please tell {selectedRequest.clientName} why you are unable to accept the connection.</p>
                        
                        <textarea 
                            value={rejectionReason}
                            onChange={(e) => setRejectionReason(e.target.value)}
                            placeholder="e.g. My schedule is currently full..."
                            className="w-full p-4 bg-slate-50 border border-slate-200 rounded-2xl focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all h-32 mb-6"
                        />
                        
                        <div className="flex gap-3">
                            <button 
                                onClick={() => setSelectedRequest(null)}
                                className="flex-1 py-3 bg-slate-100 hover:bg-slate-200 text-slate-700 font-bold rounded-2xl transition-all"
                            >
                                Cancel
                            </button>
                            <button 
                                onClick={() => handleRespond(selectedRequest.id, false)}
                                className="flex-1 py-3 bg-rose-600 hover:bg-rose-700 text-white font-bold rounded-2xl shadow-lg shadow-rose-200 transition-all"
                            >
                                Confirm Rejection
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TrainerRequests;
