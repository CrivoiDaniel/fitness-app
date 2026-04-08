import React, { useState, useEffect } from 'react';
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import appointmentsApi from '../../api/appointmentsApi';
import googleAuthApi from '../../api/googleAuthApi';
import { useAuth } from '../../context/AuthContext';
import { useTranslation } from 'react-i18next';
import AppointmentModal from '../../components/Appointments/AppointmentModal';
import { FiCalendar, FiPlus, FiLink, FiCheckCircle } from 'react-icons/fi';

const CalendarDashboard = () => {
    const { user, token } = useAuth();
    const { t } = useTranslation();
    const [events, setEvents] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedEvent, setSelectedEvent] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchAppointments();
    }, [user?.role]);

    const fetchAppointments = async () => {
        setLoading(true);
        try {
            const data = user?.role === 'Trainer' 
                ? await appointmentsApi.getTrainerAppointments(token) 
                : await appointmentsApi.getClientAppointments(token);
            
            const calendarEvents = data.map(app => ({
                id: app.id.toString(),
                title: app.title,
                start: app.startTime,
                end: app.endTime,
                extendedProps: { ...app },
                backgroundColor: user?.role === 'Trainer' ? '#fbbf24' : '#000000', // Yellow for trainer, black for client
                textColor: user?.role === 'Trainer' ? '#000000' : '#ffffff',
            }));
            setEvents(calendarEvents);
        } catch (err) {
            console.error("Fetch Error:", err);
        } finally {
            setLoading(false);
        }
    };

    const handleDateClick = (arg) => {
        if (user?.role !== 'Trainer') return;
        const endTime = new Date(arg.date.getTime() + 60 * 60 * 1000);
        setSelectedEvent({
            startTime: arg.date,
            endTime: endTime,
        });
        setIsModalOpen(true);
    };

    const handleEventClick = (arg) => {
        if (user?.role !== 'Trainer') {
            // For clients, just show info? Or maybe just ignore
            return;
        }
        setSelectedEvent(arg.event.extendedProps);
        setIsModalOpen(true);
    };

    const handleConnectGoogle = async () => {
        try {
            const { url } = await googleAuthApi.getAuthUrl(token);
            window.location.href = url;
        } catch (err) {
            console.error("Google Auth Error:", err);
            alert("Eroare la obținerea URL-ului de autorizare.");
        }
    };

    return (
        <div className="space-y-8 animate-in fade-in duration-700">
            <header className="flex flex-wrap items-center justify-between gap-6">
                <div>
                    <h1 className="text-4xl font-black italic tracking-tighter uppercase leading-none">
                        {t('calendar.title', 'Calendarul Antrenamentelor')}
                    </h1>
                    <p className="text-gray-400 uppercase tracking-widest text-xs font-bold mt-2 ml-1">
                        {t('calendar.subtitle', 'Sesiunile tale programate')}
                    </p>
                </div>

                <div className="flex gap-4">
                    {user?.role === 'Trainer' && (
                        <button 
                            onClick={() => { setSelectedEvent(null); setIsModalOpen(true); }}
                            className="bg-yellow-400 text-black px-8 py-3 rounded-2xl font-black italic text-sm flex items-center gap-3 hover:bg-yellow-300 transition-all active:scale-95 shadow-[0_10px_30px_rgba(251,191,36,0.2)] uppercase tracking-tighter"
                        >
                            <FiPlus className="text-xl" />
                            {t('calendar.newAppointment', 'Programare Nouă')}
                        </button>
                    )}
                    <button 
                        onClick={handleConnectGoogle}
                        className="bg-white border border-gray-100 px-7 py-3 rounded-2xl font-black italic text-sm flex items-center gap-3 hover:bg-gray-50 transition-all text-black uppercase tracking-tighter"
                    >
                        <FiLink className="text-yellow-500" />
                        {t('calendar.connectGoogle', 'Sync Google')}
                    </button>
                </div>
            </header>

            <div className="bg-white rounded-[3rem] p-10 shadow-[0_20px_50px_rgba(0,0,0,0.05)] border border-gray-50 transition-all hover:shadow-[0_30px_70px_rgba(0,0,0,0.08)]">
                {loading ? (
                    <div className="h-[600px] flex items-center justify-center">
                        <div className="w-12 h-12 border-4 border-yellow-400 border-t-transparent rounded-full animate-spin"></div>
                    </div>
                ) : (
                    <div className="full-calendar-custom-premium">
                        <FullCalendar
                            plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
                            initialView="timeGridWeek"
                            headerToolbar={{
                                left: 'prev,next today',
                                center: 'title',
                                right: 'dayGridMonth,timeGridWeek,timeGridDay'
                            }}
                            events={events}
                            selectable={true}
                            dateClick={handleDateClick}
                            eventClick={handleEventClick}
                            locale="ro"
                            allDaySlot={false}
                            slotMinTime="07:00:00"
                            slotMaxTime="22:00:00"
                            height="700px"
                            nowIndicator={true}
                            slotDuration="00:15:00"
                            eventClassNames="rounded-xl border-0 shadow-lg font-black italic flex flex-col p-2 overflow-hidden transition-all hover:scale-[1.02] cursor-pointer"
                            eventContent={(eventInfo) => (
                                <div className="space-y-0.5">
                                    <div className="text-[10px] opacity-80 uppercase tracking-tighter">
                                        {eventInfo.timeText}
                                    </div>
                                    <div className="truncate text-xs uppercase leading-tight font-black">
                                        {eventInfo.event.title}
                                    </div>
                                    {eventInfo.event.extendedProps.client && (
                                        <div className="text-[9px] opacity-70 italic truncate">
                                            {eventInfo.event.extendedProps.client.user?.firstName}
                                        </div>
                                    )}
                                </div>
                            )}
                        />
                    </div>
                )}
            </div>

            {/* Custom Styles for FullCalendar */}
            <style jsx global>{`
                .fc-header-toolbar {
                    margin-bottom: 2.5rem !important;
                }
                .fc-toolbar-title {
                    font-size: 1.25rem !important;
                    font-weight: 900 !important;
                    text-transform: uppercase !important;
                    font-style: italic !important;
                    letter-spacing: -0.05em !important;
                }
                .fc-button {
                    background: transparent !important;
                    border: 1px solid #f3f4f6 !important;
                    color: #000 !important;
                    font-weight: 800 !important;
                    text-transform: uppercase !important;
                    font-size: 0.7rem !important;
                    letter-spacing: 0.05em !important;
                    padding: 0.6rem 1rem !important;
                    border-radius: 12px !important;
                    transition: all 0.2s !important;
                }
                .fc-button:hover {
                    background: #f9fafb !important;
                }
                .fc-button-active {
                    background: #000 !important;
                    color: #fff !important;
                    border-color: #000 !important;
                }
                .fc-theme-standard td, .fc-theme-standard th {
                    border-color: #f3f4f6 !important;
                }
                .fc-col-header-cell {
                    padding: 1rem 0 !important;
                    background: #fcfcfc !important;
                }
                .fc-col-header-cell-cushion {
                    font-weight: 800 !important;
                    text-transform: uppercase !important;
                    font-size: 0.65rem !important;
                    color: #9ca3af !important;
                    letter-spacing: 0.1em !important;
                }
                .fc-timegrid-slot-label-cushion {
                    font-size: 0.65rem !important;
                    font-weight: 700 !important;
                    color: #9ca3af !important;
                }
            `}</style>

            {isModalOpen && (
                <AppointmentModal 
                    isOpen={isModalOpen}
                    onClose={() => setIsModalOpen(false)}
                    appointment={selectedEvent}
                    onSave={fetchAppointments}
                />
            )}
        </div>
    );
};

export default CalendarDashboard;
