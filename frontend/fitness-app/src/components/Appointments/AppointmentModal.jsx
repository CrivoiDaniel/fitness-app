import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useAuth } from '../../context/AuthContext';
import { getAllClients } from '../../api/admin/clients';
import appointmentsApi from '../../api/appointmentsApi';
import { FiX, FiCalendar, FiClock, FiUser, FiFileText } from 'react-icons/fi';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";

const AppointmentModal = ({ isOpen, onClose, appointment, onSave }) => {
    const { token } = useAuth();
    const { t } = useTranslation();
    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(false);
    
    const [formData, setFormData] = useState({
        clientId: '',
        title: '',
        description: '',
        startTime: new Date(),
        endTime: new Date(new Date().getTime() + 60 * 60 * 1000)
    });

    useEffect(() => {
        if (isOpen) {
            fetchClients();
            if (appointment) {
                setFormData({
                    clientId: appointment.clientId || '',
                    title: appointment.title || '',
                    description: appointment.description || '',
                    startTime: new Date(appointment.startTime),
                    endTime: new Date(appointment.endTime)
                });
            }
        }
    }, [isOpen, appointment]);

    const fetchClients = async () => {
        try {
            const data = await getAllClients(token);
            setClients(Array.isArray(data) ? data : []);
        } catch (err) {
            console.error("Fetch Clients Error:", err);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            if (appointment?.id) {
                await appointmentsApi.update(token, appointment.id, formData);
            } else {
                await appointmentsApi.create(token, formData);
            }
            onSave();
            onClose();
        } catch (err) {
            console.error("Save Error:", err);
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async () => {
        if (!appointment?.id) return;
        if (!window.confirm(t('common.confirmDelete'))) return;
        
        setLoading(true);
        try {
            await appointmentsApi.delete(token, appointment.id);
            onSave();
            onClose();
        } catch (err) {
            console.error("Delete Error:", err);
        } finally {
            setLoading(false);
        }
    };

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/60 backdrop-blur-md animate-in fade-in duration-300">
            <div className="bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl border border-gray-100 overflow-hidden animate-in zoom-in-95 duration-300">
                <header className="px-8 py-6 border-b flex items-center justify-between">
                    <div>
                        <h2 className="text-2xl font-black italic tracking-tighter uppercase">
                            {appointment?.id ? t('calendar.editAppointment', 'Editează Programarea') : t('calendar.newAppointment', 'Programare Nouă')}
                        </h2>
                        <p className="text-gray-500 uppercase tracking-widest text-[10px] font-bold">Configurarea sesiunii</p>
                    </div>
                    <button onClick={onClose} className="p-2 hover:bg-gray-100 rounded-full transition-all">
                        <FiX className="text-xl" />
                    </button>
                </header>

                <form onSubmit={handleSubmit} className="p-8 space-y-6">
                    <div className="space-y-4">
                        {/* Client Select */}
                        <div className="space-y-1">
                            <label className="text-[10px] font-black uppercase tracking-widest text-gray-400 flex items-center gap-1">
                                <FiUser /> {t('calendar.client', 'Client')}
                            </label>
                            <select 
                                required
                                value={formData.clientId}
                                onChange={(e) => setFormData({ ...formData, clientId: e.target.value })}
                                className="w-full bg-gray-50 border border-gray-100 rounded-2xl px-5 py-4 font-bold text-sm focus:ring-2 focus:ring-yellow-400 outline-none transition-all appearance-none"
                            >
                                <option value="">{t('calendar.selectClient', 'Selectează un client')}</option>
                                {clients.map(c => (
                                    <option key={c.clientId} value={c.clientId}>{c.firstName} {c.lastName}</option>
                                ))}
                            </select>
                        </div>

                        {/* Title */}
                        <div className="space-y-1">
                            <label className="text-[10px] font-black uppercase tracking-widest text-gray-400 flex items-center gap-1">
                                <FiFileText /> {t('calendar.title', 'Titlu')}
                            </label>
                            <input 
                                required
                                type="text"
                                placeholder={t('calendar.titlePlaceholder', 'ex: Cardio Session, Legs Day...')}
                                value={formData.title}
                                onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                                className="w-full bg-gray-50 border border-gray-100 rounded-2xl px-5 py-4 font-bold text-sm focus:ring-2 focus:ring-yellow-400 outline-none transition-all"
                            />
                        </div>

                        {/* Description */}
                        <div className="space-y-1">
                            <label className="text-[10px] font-black uppercase tracking-widest text-gray-400">
                                {t('calendar.description', 'Descriere')} (Optional)
                            </label>
                            <textarea 
                                value={formData.description}
                                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                                className="w-full bg-gray-50 border border-gray-100 rounded-2xl px-5 py-4 font-bold text-sm focus:ring-2 focus:ring-yellow-400 outline-none transition-all min-h-[100px]"
                            />
                        </div>

                        {/* Date & Time Grid */}
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-1">
                                <label className="text-[10px] font-black uppercase tracking-widest text-gray-400 flex items-center gap-1">
                                    <FiClock /> {t('calendar.start', 'Început')}
                                </label>
                                <DatePicker
                                    selected={formData.startTime}
                                    onChange={(date) => setFormData({ ...formData, startTime: date })}
                                    showTimeSelect
                                    timeFormat="HH:mm"
                                    timeIntervals={15}
                                    dateFormat="Pp"
                                    className="w-full bg-gray-50 border border-gray-100 rounded-2xl px-5 py-4 font-bold text-sm focus:ring-2 focus:ring-yellow-400 outline-none transition-all"
                                />
                            </div>
                            <div className="space-y-1">
                                <label className="text-[10px] font-black uppercase tracking-widest text-gray-400 flex items-center gap-1">
                                    <FiClock /> {t('calendar.end', 'Sfârșit')}
                                </label>
                                <DatePicker
                                    selected={formData.endTime}
                                    onChange={(date) => setFormData({ ...formData, endTime: date })}
                                    showTimeSelect
                                    timeFormat="HH:mm"
                                    timeIntervals={15}
                                    dateFormat="Pp"
                                    className="w-full bg-gray-50 border border-gray-100 rounded-2xl px-5 py-4 font-bold text-sm focus:ring-2 focus:ring-yellow-400 outline-none transition-all"
                                />
                            </div>
                        </div>
                    </div>

                    <footer className="flex gap-3 pt-6">
                        {appointment?.id && (
                            <button 
                                type="button"
                                onClick={handleDelete}
                                disabled={loading}
                                className="flex-1 bg-red-50 text-red-500 font-black py-4 rounded-2xl hover:bg-red-100 transition-all uppercase tracking-tighter italic"
                            >
                                {t('common.delete', 'Șterge')}
                            </button>
                        )}
                        <button 
                            type="submit"
                            disabled={loading}
                            className="flex-[2] bg-black text-white font-black py-4 rounded-2xl hover:bg-yellow-400 hover:text-black transition-all duration-300 uppercase tracking-tighter italic"
                        >
                            {loading ? t('common.saving', 'Se salvează...') : (appointment?.id ? t('common.saveChanges', 'Salvează Modificările') : t('calendar.create', 'Creează Programarea'))}
                        </button>
                    </footer>
                </form>
            </div>
        </div>
    );
};

export default AppointmentModal;
