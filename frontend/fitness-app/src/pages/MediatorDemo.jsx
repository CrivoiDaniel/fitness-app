import React, { useState, useEffect, useRef } from 'react';
import designPatternsApi from '../api/lab/designPatternsApi';
import { useAuth } from '../context/AuthContext';

const MediatorDemo = () => {
    const { user } = useAuth();
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState("");
    const [loading, setLoading] = useState(true);
    const messagesEndRef = useRef(null);

    const scrollToBottom = () => {
        messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
    };

    useEffect(() => {
        fetchMessages();
        const interval = setInterval(fetchMessages, 3000); // Polling pentru simplitate
        return () => clearInterval(interval);
    }, []);

    useEffect(() => {
        scrollToBottom();
    }, [messages]);

    const fetchMessages = async () => {
        try {
            const data = await designPatternsApi.mediator.getMessages();
            setMessages(data);
        } catch (error) {
            console.error("Error fetching messages", error);
        } finally {
            setLoading(false);
        }
    };

    const handleSendMessage = async (e) => {
        e.preventDefault();
        if (!newMessage.trim()) return;

        const senderName = user ? `${user.firstName} ${user.lastName}` : "Anonymous";
        const senderRole = user?.role || "Client";

        try {
            await designPatternsApi.mediator.sendMessage(senderName, senderRole, newMessage);
            setNewMessage("");
            fetchMessages();
        } catch (error) {
            console.error("Error sending message", error);
        }
    };

    return (
        <div className="p-8 max-w-4xl mx-auto h-[calc(100vh-100px)] flex flex-col">
            <div className="mb-6">
                <h1 className="text-3xl font-bold text-slate-900">Chat Mediator</h1>
                <p className="text-slate-500">Toate mesajele trec printr-un Mediator centralizat (ChatRoomMediator).</p>
            </div>

            <div className="flex-1 bg-white/60 backdrop-blur-md rounded-3xl border border-white/20 shadow-xl overflow-hidden flex flex-col">
                {/* Header Chat */}
                <div className="p-4 border-b border-slate-100 bg-white/40 flex items-center gap-3">
                    <div className="w-3 h-3 bg-emerald-500 rounded-full animate-pulse"></div>
                    <span className="font-bold text-slate-700 uppercase text-xs tracking-wider">Live Chat Room</span>
                </div>

                {/* Zona Mesaje */}
                <div className="flex-1 overflow-y-auto p-6 space-y-4">
                    {loading ? (
                        <div className="flex justify-center items-center h-full">
                            <div className="w-8 h-8 border-4 border-indigo-200 border-t-indigo-600 rounded-full animate-spin"></div>
                        </div>
                    ) : messages.length === 0 ? (
                        <div className="text-center py-20 text-slate-400 italic">
                            Niciun mesaj încă. Sparge gheața!
                        </div>
                    ) : (
                        messages.map((msg, index) => {
                            const isMe = user && msg.from === `${user.firstName} ${user.lastName}`;
                            return (
                                <div key={index} className={`flex flex-col ${isMe ? 'items-end' : 'items-start'}`}>
                                    <div className="flex items-center gap-2 mb-1">
                                        <span className="text-[10px] font-bold text-slate-400 uppercase">{msg.from} ({msg.role})</span>
                                        <span className="text-[10px] text-slate-300">{msg.timestamp}</span>
                                    </div>
                                    <div className={`max-w-[80%] px-4 py-2 rounded-2xl text-sm ${
                                        isMe 
                                        ? 'bg-indigo-600 text-white rounded-tr-none shadow-lg shadow-indigo-100' 
                                        : 'bg-white text-slate-700 rounded-tl-none border border-slate-100 shadow-sm'
                                    }`}>
                                        {msg.content}
                                    </div>
                                </div>
                            );
                        })
                    )}
                    <div ref={messagesEndRef} />
                </div>

                {/* Input Area */}
                <form onSubmit={handleSendMessage} className="p-4 bg-slate-50/50 border-t border-slate-100 flex gap-2">
                    <input 
                        type="text" 
                        value={newMessage}
                        onChange={(e) => setNewMessage(e.target.value)}
                        placeholder="Scrie un mesaj..."
                        className="flex-1 bg-white border border-slate-200 rounded-xl px-4 py-2 outline-none focus:ring-2 focus:ring-indigo-500 transition-all text-sm"
                    />
                    <button 
                        type="submit"
                        className="bg-indigo-600 text-white px-6 py-2 rounded-xl font-bold hover:bg-indigo-700 transition-all shadow-lg shadow-indigo-200 flex items-center gap-2"
                    >
                        Trimite <i className="fas fa-paper-plane text-xs"></i>
                    </button>
                </form>
            </div>

            <div className="mt-4 bg-amber-50 border border-amber-100 p-4 rounded-2xl flex gap-3 items-start">
                <i className="fas fa-info-circle text-amber-500 mt-1"></i>
                <p className="text-xs text-amber-800 leading-relaxed">
                    <strong>Cum funcționează Mediatorul aici:</strong> Când apeși "Trimite", mesajul nu ajunge direct la ceilalți utilizatori. El este trimis către <code>ChatRoomMediator</code>. Acesta decide cine ar trebui să vadă mesajul, îl stochează și îl livrează participanților înregistrați. Componentele UI nu știu nimic una de cealaltă.
                </p>
            </div>
        </div>
    );
};

export default MediatorDemo;
