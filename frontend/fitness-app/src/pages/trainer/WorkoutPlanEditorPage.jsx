import React, { useState, useEffect, useCallback } from "react";
import { useAuth } from "../../context/AuthContext";
import { workoutEditorApi } from "../../api/lab/workoutEditor";
import { 
  LuPlus, 
  LuRotateCcw, 
  LuRotateCw, 
  LuTrash2, 
  LuHistory, 
  LuActivity,
  LuChevronRight,
  LuClipboardList,
  LuUser,
  LuSearch,
  LuCamera,
  LuDownload,
  LuSave,
  LuPlay,
  LuSkipForward,
  LuListOrdered,
  LuZap,
  LuRefreshCw
} from "react-icons/lu";

export default function WorkoutPlanEditorPage() {
  const { token } = useAuth();
  const [clients, setClients] = useState([]);
  const [selectedClient, setSelectedClient] = useState(null);
  const [sessionStarted, setSessionStarted] = useState(false);
  const [state, setState] = useState(null);
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({ name: "", sets: 3, reps: 10 });
  const [planName, setPlanName] = useState("Plan Nou");
  const [lastAction, setLastAction] = useState("");
  const [search, setSearch] = useState("");
  const [checkpointName, setCheckpointName] = useState("Versiune Noua");
  const [iteratorType, setIteratorType] = useState("sequential");
  const [currentNav, setCurrentNav] = useState(null);

  // FETCH CLIENTS
  useEffect(() => {
    const fetchClients = async () => {
      try {
        const data = await workoutEditorApi.getClients(token);
        setClients(data);
      } catch (err) {
        console.error("Failed to fetch clients:", err);
      }
    };
    if (token) fetchClients();
  }, [token]);

  const fetchState = useCallback(async () => {
    try {
      const data = await workoutEditorApi.getState(token);
      setState(data);
    } catch (err) {
      console.error("Failed to fetch state:", err);
    }
  }, [token]);

  const startSession = async () => {
    if (!selectedClient) return;
    setLoading(true);
    try {
      const resp = await workoutEditorApi.startSession(token, selectedClient.userId, planName);
      setSessionStarted(true);
      await fetchState();
      
      if (resp.isResume) {
        setLastAction("Sesiune existentă restaurată");
      }
    } catch (err) {
      alert(err.message);
    } finally {
      setLoading(false);
    }
  };

  const onSave = async () => {
    setLoading(true);
    try {
      await workoutEditorApi.savePlan(token);
      alert("Plan salvat cu succes în baza de date!");
    } catch (err) {
      alert("Eroare la salvare: " + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleAction = async (promise, name) => {
    setLoading(true);
    try {
      await promise;
      setLastAction(name);
      await fetchState();
    } catch (err) {
      alert(err.message);
    } finally {
      setLoading(false);
    }
  };

  const onAdd = (e) => {
    e.preventDefault();
    if (!form.name) return;
    handleAction(
      workoutEditorApi.addExercise(token, form.name, form.sets, form.reps),
      `Adăugat: ${form.name}`
    );
    setForm({ ...form, name: "" });
  };

  const onCreateCheckpoint = () => {
    if (!checkpointName) return;
    handleAction(
      workoutEditorApi.createCheckpoint(token, checkpointName),
      `Checkpoint salvat: ${checkpointName}`
    );
  };

  const onLoadCheckpoint = (idx, name) => {
    if (!window.confirm(`Ești sigur că vrei să încarci versiunea "${name}"? Istoricul curent de Undo/Redo va fi resetat.`)) return;
    handleAction(
      workoutEditorApi.loadCheckpoint(token, idx),
      `Restaurat la versiunea: ${name}`
    );
  };

  const onStartNavigation = () => {
    handleAction(
      workoutEditorApi.startNavigation(token, iteratorType),
      `Navigarea a început (${iteratorType})`
    ).then(res => {
      if (res?.exercise) setCurrentNav(res.exercise);
    });
  };

  const onNextExercise = () => {
    handleAction(
      workoutEditorApi.nextExercise(token),
      `Următorul exercițiu...`
    ).then(res => {
        if (res?.finished) {
           setCurrentNav(null);
           setLastAction("Antrenament Finalizat!");
        } else if (res?.exercise) {
           setCurrentNav(res.exercise);
        }
    });
  };

  const onResetNavigation = () => {
    handleAction(
      workoutEditorApi.resetNavigation(token),
      `Navigarea a fost resetată`
    ).then(() => setCurrentNav(null));
  };

  const filteredClients = clients.filter(c => 
    `${c.firstName} ${c.lastName}`.toLowerCase().includes(search.toLowerCase())
  );

  // --- STEP 1: SELECT CLIENT ---
  if (!sessionStarted) {
    return (
      <div className="min-h-screen bg-neutral-50 p-6 lg:p-10">
        <div className="max-w-4xl mx-auto">
          <header className="mb-10">
            <h1 className="text-3xl font-black text-neutral-950 tracking-tight">Creează Plan de Antrenament</h1>
            <p className="text-neutral-500 mt-2 font-medium">Selectează un client pentru a începe editarea cu suport Undo/Redo.</p>
          </header>

          <div className="bg-white rounded-3xl p-8 border border-neutral-200 shadow-sm space-y-6">
            <div className="relative">
              <LuSearch className="absolute left-4 top-1/2 -translate-y-1/2 text-neutral-400" size={20} />
              <input 
                type="text" 
                placeholder="Caută client după nume..."
                value={search}
                onChange={e => setSearch(e.target.value)}
                className="w-full pl-12 pr-4 py-4 rounded-2xl bg-neutral-100 border-transparent focus:bg-white focus:ring-2 focus:ring-indigo-500 transition-all font-medium"
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 max-h-[400px] overflow-y-auto pr-2 custom-scrollbar">
              {filteredClients.map(client => (
                <button 
                  key={client.userId}
                  onClick={() => setSelectedClient(client)}
                  className={`flex items-center gap-4 p-4 rounded-2xl border transition-all text-left ${
                    selectedClient?.userId === client.userId 
                    ? "border-indigo-500 bg-indigo-50 ring-2 ring-indigo-500/20" 
                    : "border-neutral-100 bg-neutral-50 hover:border-neutral-200"
                  }`}
                >
                  <div className="w-12 h-12 rounded-full bg-white flex items-center justify-center text-indigo-600 border border-neutral-100 shadow-sm">
                    <LuUser size={24} />
                  </div>
                  <div>
                    <div className="font-bold text-neutral-900">{client.firstName} {client.lastName}</div>
                    <div className="text-xs text-neutral-500 font-medium">{client.email}</div>
                  </div>
                </button>
              ))}
            </div>

            {selectedClient && (
              <div className="pt-6 border-t border-neutral-100 animate-in fade-in slide-in-from-bottom-2">
                <label className="block text-xs font-black uppercase text-neutral-400 mb-3 ml-1">Nume Program Antrenament</label>
                <div className="flex gap-4">
                  <input 
                    type="text" 
                    value={planName}
                    onChange={e => setPlanName(e.target.value)}
                    className="flex-1 px-5 py-4 rounded-2xl bg-neutral-100 border-transparent focus:bg-white focus:ring-2 focus:ring-indigo-500 transition-all font-bold"
                  />
                  <button 
                    onClick={startSession}
                    disabled={loading}
                    className="px-8 py-4 bg-neutral-950 text-white rounded-2xl font-black text-sm uppercase tracking-widest hover:bg-neutral-800 transition-all shadow-lg"
                  >
                    Începe Editarea
                  </button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    );
  }

  // --- STEP 2: EDIT PLAN (The Command Interface) ---
  return (
    <div className="min-h-screen bg-neutral-50 p-6 lg:p-10">
      <div className="max-w-6xl mx-auto space-y-8">
        
        <header className="flex flex-col md:flex-row md:items-center justify-between gap-6">
          <div className="flex items-center gap-6">
            <button 
              onClick={() => setSessionStarted(false)}
              className="p-3 rounded-2xl bg-white border border-neutral-200 text-neutral-500 hover:text-neutral-900 transition-all shadow-sm"
              title="Înapoi la selecție"
            >
              <LuChevronRight size={24} className="rotate-180" />
            </button>
            <div>
              <div className="flex items-center gap-2 text-indigo-600 font-bold tracking-tight mb-1 uppercase text-[10px] tracking-[0.2em]">
                <LuActivity size={14} />
                Client: {selectedClient.firstName} {selectedClient.lastName}
              </div>
              <h1 className="text-4xl font-black text-neutral-950 tracking-tight leading-none italic uppercase">
                {planName}
              </h1>
            </div>
          </div>

          <div className="flex items-center gap-3">
            <button 
              onClick={() => handleAction(workoutEditorApi.undo(token), "Undo Action")}
              disabled={!state?.canUndo || loading}
              className="flex items-center gap-2 px-6 py-4 rounded-2xl bg-white border border-neutral-200 hover:border-neutral-300 hover:bg-neutral-50 disabled:opacity-40 disabled:cursor-not-allowed transition-all font-bold text-sm shadow-sm"
            >
              <LuRotateCcw size={18} />
              Undo
            </button>
            <button 
              onClick={() => handleAction(workoutEditorApi.redo(token), "Redo Action")}
              disabled={!state?.canRedo || loading}
              className="flex items-center gap-2 px-6 py-4 rounded-2xl bg-white border border-neutral-200 hover:border-neutral-300 hover:bg-neutral-50 disabled:opacity-40 disabled:cursor-not-allowed transition-all font-bold text-sm shadow-sm"
            >
              <LuRotateCw size={18} />
              Redo
            </button>
            <button 
              onClick={() => handleAction(workoutEditorApi.reset(token), "Reset Session")}
              className="p-4 rounded-2xl bg-red-50 text-red-600 hover:bg-red-100 transition-all border border-red-100"
              title="Șterge tot"
            >
              <LuTrash2 size={24} />
            </button>
          </div>
        </header>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          
          <div className="lg:col-span-2 space-y-8">
            <section className="bg-white rounded-3xl p-8 border border-neutral-200 shadow-sm">
              <h2 className="text-xl font-bold mb-6 flex items-center gap-2">
                <LuPlus size={22} className="text-indigo-600" />
                Adaugă Exercițiu în Plan
              </h2>
              <form onSubmit={onAdd} className="grid grid-cols-1 md:grid-cols-4 gap-4">
                <div className="md:col-span-2">
                  <label className="block text-xs font-black text-neutral-400 uppercase mb-2 ml-1">Exercițiu</label>
                  <input 
                    type="text" 
                    value={form.name}
                    onChange={e => setForm({...form, name: e.target.value})}
                    placeholder="ex: Bench Press"
                    className="w-full px-5 py-4 rounded-2xl bg-neutral-100 border-transparent focus:bg-white focus:ring-2 focus:ring-indigo-500 focus:border-transparent transition-all font-bold"
                  />
                </div>
                <div>
                  <label className="block text-xs font-black text-neutral-400 uppercase mb-2 ml-1">Sets</label>
                  <input 
                    type="number" 
                    value={form.sets}
                    onChange={e => setForm({...form, sets: parseInt(e.target.value)})}
                    className="w-full px-5 py-4 rounded-2xl bg-neutral-100 border-transparent focus:bg-white focus:ring-2 focus:ring-indigo-500 transition-all font-bold text-center"
                  />
                </div>
                <div>
                  <label className="block text-xs font-black text-neutral-400 uppercase mb-2 ml-1">Reps</label>
                  <input 
                    type="number" 
                    value={form.reps}
                    onChange={e => setForm({...form, reps: parseInt(e.target.value)})}
                    className="w-full px-5 py-4 rounded-2xl bg-neutral-100 border-transparent focus:bg-white focus:ring-2 focus:ring-indigo-500 transition-all font-bold text-center"
                  />
                </div>
                <button 
                  type="submit"
                  disabled={!form.name || loading}
                  className="md:col-span-4 bg-neutral-950 text-white py-5 rounded-3xl font-black text-xs uppercase tracking-[0.2em] hover:bg-neutral-800 transition-all shadow-xl hover:shadow-neutral-200"
                >
                  Execută Comanda de Adăugare
                </button>
              </form>
            </section>

            <section className="bg-white rounded-3xl p-8 border border-neutral-200 shadow-sm min-h-[500px]">
              <div className="flex items-center justify-between mb-8">
                <h2 className="text-xl font-bold flex items-center gap-2 uppercase tracking-tight">
                  <LuClipboardList size={22} className="text-indigo-600" />
                  Estructură Plan curent
                </h2>
                <span className="px-4 py-1.5 rounded-full bg-neutral-100 text-neutral-600 text-xs font-black uppercase tracking-wider border border-neutral-200">
                  {state?.exercises?.length || 0} Segmentate
                </span>
              </div>

              {!state?.exercises?.length ? (
                <div className="flex flex-col items-center justify-center py-24 text-neutral-300">
                  <LuClipboardList size={64} strokeWidth={1} className="mb-4 opacity-10" />
                  <p className="font-bold uppercase tracking-widest text-[10px]">Lista este goală</p>
                </div>
              ) : (
                <div className="space-y-4">
                  {state.exercises.map((ex, idx) => (
                    <div 
                      key={idx} 
                      className="group flex items-center justify-between p-6 rounded-3xl border border-neutral-100 hover:border-primary-500 hover:bg-primary-50/30 transition-all animate-in zoom-in-95 duration-300"
                    >
                      <div className="flex items-center gap-5">
                        <div className="w-12 h-12 rounded-2xl bg-neutral-950 text-white flex items-center justify-center font-black text-sm shadow-lg shadow-neutral-300">
                          {idx + 1}
                        </div>
                        <div>
                          <h4 className="font-black text-neutral-950 uppercase tracking-tight text-lg leading-tight">
                            {ex.exerciseName}
                          </h4>
                          <span className="px-2 py-0.5 rounded bg-neutral-100 text-[10px] font-black uppercase text-neutral-500 tracking-tighter mr-2">Programat</span>
                          <span className="text-xs text-indigo-600 font-black uppercase">{ex.sets} Sets × {ex.reps} Reps</span>
                        </div>
                      </div>
                      <LuChevronRight size={20} className="text-neutral-200 group-hover:text-indigo-500 group-hover:translate-x-1 transition-all" />
                    </div>
                  ))}
                  <div className="pt-10 flex justify-center">
                    <button 
                      onClick={onSave}
                      disabled={loading}
                      className="px-12 py-5 bg-indigo-600 text-white rounded-3xl font-black text-xs uppercase tracking-[0.3em] hover:bg-indigo-700 transition-all shadow-xl shadow-indigo-200 disabled:opacity-50"
                    >
                      {loading ? "Se salvează..." : "Salvează & Trimite Clientului"}
                    </button>
                  </div>
                </div>
              )}
            </section>
          </div>

          <aside className="space-y-6">
            <div className="bg-neutral-950 text-white rounded-[40px] p-8 shadow-2xl relative overflow-hidden">
              <div className="absolute top-0 right-0 p-8 opacity-5">
                <LuHistory size={120} />
              </div>
              <h3 className="text-lg font-black uppercase tracking-[0.2em] flex items-center gap-2 mb-8 text-indigo-400 relative z-10">
                <LuHistory size={20} />
                Command Logs
              </h3>
              
              <div className="space-y-8 relative z-10">
                <div>
                  <div className="flex items-center justify-between mb-4">
                    <div className="text-[10px] uppercase font-black text-neutral-500 tracking-widest">History (Undo Stack)</div>
                    <div className="text-[10px] font-black text-indigo-500">{state?.undoHistory?.length || 0}</div>
                  </div>
                  {!state?.undoHistory?.length ? (
                    <div className="bg-white/5 rounded-2xl p-4 text-[10px] text-neutral-600 font-bold uppercase tracking-widest text-center border border-white/5">No commands recorded</div>
                  ) : (
                    <div className="space-y-2">
                       {state.undoHistory.map((cmd, i) => (
                         <div key={i} className="text-[10px] font-black uppercase tracking-tight flex items-center gap-3 p-3 rounded-2xl bg-white/5 border border-white/10 hover:bg-white/10 transition-colors">
                            <div className="w-2 h-2 rounded-full bg-green-500 shadow-sm shadow-green-500/50" />
                            {cmd}
                         </div>
                       ))}
                    </div>
                  )}
                </div>

                <div className="pt-8 border-t border-white/10">
                   <div className="flex items-center justify-between mb-4">
                    <div className="text-[10px] uppercase font-black text-neutral-500 tracking-widest">Buffer (Redo Stack)</div>
                    <div className="text-[10px] font-black text-orange-500">{state?.redoHistory?.length || 0}</div>
                  </div>
                  {!state?.redoHistory?.length ? (
                    <div className="bg-white/5 rounded-2xl p-4 text-[10px] text-neutral-600 font-bold uppercase tracking-widest text-center border border-white/5 italic opacity-40">Redo stack empty</div>
                  ) : (
                    <div className="space-y-2 opacity-60">
                       {state.redoHistory.map((cmd, i) => (
                         <div key={i} className="text-[10px] font-black uppercase tracking-tight flex items-center gap-3 p-3 rounded-2xl bg-white/5 border border-white/5">
                            <div className="w-2 h-2 rounded-full bg-orange-500 opacity-50" />
                            {cmd}
                         </div>
                       ))}
                    </div>
                  )}
                </div>
              </div>

              {lastAction && (
                <div className="mt-10 p-5 rounded-3xl bg-indigo-500/10 border border-indigo-500/20 animate-in slide-in-from-bottom-4">
                   <div className="text-[10px] uppercase font-black text-indigo-400 mb-1 tracking-widest">Status Curent</div>
                   <div className="text-xs font-black text-white italic">"{lastAction}"</div>
                </div>
              )}
            </div>

            {/* ========== MEMENTO SECTION: CHECKPOINTS ========== */}
            <div className="bg-white rounded-[40px] p-8 border border-neutral-200 shadow-sm space-y-6">
              <h3 className="text-lg font-black uppercase tracking-tight flex items-center gap-2 text-neutral-900 leading-tight">
                <LuCamera size={20} className="text-indigo-600" />
                Versiuni Plan (Memento)
              </h3>
              
              <div className="space-y-4">
                <div className="flex gap-2">
                  <input 
                    type="text" 
                    value={checkpointName}
                    onChange={e => setCheckpointName(e.target.value)}
                    placeholder="Numele versiunii..."
                    className="flex-1 px-4 py-3 rounded-xl bg-neutral-100 border-transparent text-xs font-bold focus:bg-white focus:ring-2 focus:ring-indigo-500 transition-all"
                  />
                  <button 
                    onClick={onCreateCheckpoint}
                    disabled={loading}
                    className="p-3 bg-indigo-600 text-white rounded-xl hover:bg-indigo-700 transition-all shadow-lg shadow-indigo-100"
                    title="Salvează Snapshot"
                  >
                    <LuSave size={18} />
                  </button>
                </div>

                <div className="space-y-2 max-h-[300px] overflow-y-auto pr-2 custom-scrollbar">
                  {!state?.checkpoints?.length ? (
                    <div className="py-8 text-center border-2 border-dashed border-neutral-100 rounded-2xl text-neutral-400 text-[10px] font-black uppercase tracking-widest">
                      Nicio versiune salvată
                    </div>
                  ) : (
                    state.checkpoints.map((m, i) => (
                      <div key={i} className="flex items-center justify-between p-3 rounded-2xl bg-neutral-50 border border-neutral-100 group hover:border-indigo-200 transition-all">
                        <div className="min-w-0">
                          <div className="text-[11px] font-black text-neutral-900 truncate uppercase leading-tight">{m.name}</div>
                          <div className="text-[9px] text-neutral-400 font-bold">{new Date(m.createdAt).toLocaleTimeString()}</div>
                        </div>
                        <button 
                          onClick={() => onLoadCheckpoint(i, m.name)}
                          disabled={loading}
                          className="p-2.5 rounded-xl bg-white text-indigo-600 border border-neutral-100 hover:bg-indigo-600 hover:text-white transition-all shadow-sm opacity-0 group-hover:opacity-100"
                        >
                          <LuDownload size={14} />
                        </button>
                      </div>
                    ))
                  )}
                </div>
              </div>
            </div>

            {/* ========== ITERATOR SECTION: NAVIGATOR ========== */}
            <div className="bg-neutral-900 rounded-[40px] p-8 border border-white/5 shadow-2xl relative overflow-hidden group">
              {/* Background Glow */}
              <div className="absolute -top-10 -right-10 w-32 h-32 bg-indigo-500/10 blur-3xl group-hover:bg-indigo-500/20 transition-all" />
              
              <div className="relative space-y-6">
                <h3 className="text-lg font-black uppercase tracking-tight flex items-center gap-2 text-white leading-tight">
                  <LuPlay size={20} className="text-indigo-400" />
                  Mod Navigator (Iterator)
                </h3>

                <div className="space-y-4">
                  <div className="flex bg-white/5 p-1 rounded-2xl border border-white/5">
                    <button 
                      onClick={() => setIteratorType("sequential")}
                      className={`flex-1 flex items-center justify-center gap-2 py-2.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all ${iteratorType === 'sequential' ? 'bg-indigo-600 text-white shadow-lg' : 'text-neutral-500 hover:text-white'}`}
                    >
                      <LuListOrdered size={14} /> Secvențial
                    </button>
                    <button 
                      onClick={() => setIteratorType("intensity")}
                      className={`flex-1 flex items-center justify-center gap-2 py-2.5 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all ${iteratorType === 'intensity' ? 'bg-indigo-600 text-white shadow-lg' : 'text-neutral-500 hover:text-white'}`}
                    >
                      <LuZap size={14} /> Intensitate
                    </button>
                  </div>

                  {currentNav ? (
                    <div className="bg-white/5 rounded-3xl p-5 border border-indigo-500/20 animate-in zoom-in-95">
                      <div className="text-[9px] uppercase font-bold text-indigo-400 mb-2 tracking-widest">Acum urmează:</div>
                      <div className="text-lg font-black text-white leading-tight mb-1">{currentNav.exerciseName}</div>
                      <div className="text-[10px] font-black text-neutral-400 uppercase tracking-tighter">
                        {currentNav.sets} Seturi &bull; {currentNav.reps} Repetări
                      </div>
                      
                      <div className="mt-6 flex gap-3">
                        <button 
                          onClick={onNextExercise}
                          className="flex-1 py-3 bg-white text-neutral-900 rounded-2xl text-[10px] font-black uppercase tracking-widest hover:bg-indigo-400 transition-all flex items-center justify-center gap-2"
                        >
                          Următorul <LuSkipForward size={14} />
                        </button>
                        <button 
                          onClick={onResetNavigation}
                          className="p-3 bg-white/10 text-white rounded-2xl hover:bg-red-500/20 hover:text-red-400 transition-all"
                        >
                          <LuRefreshCw size={14} />
                        </button>
                      </div>
                    </div>
                  ) : (
                    <button 
                      onClick={onStartNavigation}
                      className="w-full py-5 bg-indigo-600/20 border border-indigo-500/30 text-indigo-400 rounded-3xl text-xs font-black uppercase tracking-[0.2em] hover:bg-indigo-600 hover:text-white transition-all group"
                    >
                      Start Antrenament
                    </button>
                  )}
                </div>
              </div>
            </div>

            <div className="bg-white rounded-3xl p-8 border border-neutral-200 shadow-sm">
               <h4 className="text-xs font-black uppercase tracking-widest text-neutral-400 mb-4">De ce folosim Command?</h4>
               <p className="text-xs font-medium text-neutral-600 leading-relaxed italic">
                 "Acest pattern ne permite să decuplăm codul care dă comanda de codul care o execută. Rezultatul? O interfață extrem de rapidă, stabilă și cu suport nativ pentru anularea greșelilor (Undo)."
               </p>
            </div>
          </aside>

        </div>
      </div>
    </div>
  );
}
