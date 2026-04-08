import React, { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import googleAuthApi from '../../api/googleAuthApi';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../context/AuthContext';

const GoogleCallback = () => {
    const { token } = useAuth();
    const [searchParams] = useSearchParams();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    const { t } = useTranslation();

    useEffect(() => {
        const code = searchParams.get('code');
        if (code) {
            handleCallback(code);
        } else {
            setError(t('googleAuth.noCode', "Nu a fost recepționat niciun cod de autorizare."));
            setLoading(false);
        }
    }, [searchParams]);

    const handleCallback = async (code) => {
        try {
            await googleAuthApi.callback(token, code);
            // Redirect to the calendar page after successful connection
            navigate('/dashboard/trainer/calendar'); 
        } catch (err) {
            console.error("Google Auth Error:", err);
            setError(err.response?.data?.message || t('googleAuth.failed', "Conectarea la Google Calendar a eșuat."));
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-black text-white p-6 font-sans">
            <div className="max-w-md w-full bg-white/5 border border-white/10 p-10 rounded-[2.5rem] text-center backdrop-blur-2xl shadow-2xl">
                {loading ? (
                    <div className="space-y-8">
                        <div className="relative w-20 h-20 mx-auto">
                            <div className="absolute inset-0 border-4 border-yellow-400/20 rounded-full"></div>
                            <div className="absolute inset-0 border-4 border-yellow-400 border-t-transparent rounded-full animate-spin"></div>
                        </div>
                        <div className="space-y-2">
                            <h2 className="text-3xl font-black tracking-tighter italic uppercase">{t('googleAuth.connecting', "Se conectează...")}</h2>
                            <p className="text-white/40 text-sm uppercase tracking-widest">{t('googleAuth.pleaseWait', "Vă rugăm așteptați")}</p>
                        </div>
                    </div>
                ) : (
                    <div className="space-y-8 animate-in fade-in zoom-in duration-500">
                        <div className="w-20 h-20 bg-red-500/20 text-red-500 rounded-full flex items-center justify-center mx-auto text-4xl shadow-[0_0_50px_rgba(239,68,68,0.3)]">
                            ⚠️
                        </div>
                        <div className="space-y-2">
                            <h2 className="text-3xl font-black tracking-tighter italic uppercase text-red-500">{t('googleAuth.error', "Eroare")}</h2>
                            <p className="text-white/60 leading-relaxed">{error}</p>
                        </div>
                        <button 
                            onClick={() => navigate('/dashboard')}
                            className="w-full bg-white text-black font-black py-5 rounded-2xl hover:bg-yellow-400 transition-all duration-500 transform hover:scale-[1.02] active:scale-95 uppercase tracking-tighter italic"
                        >
                            {t('common.backToDashboard', "Înapoi la Dashboard")}
                        </button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default GoogleCallback;
