/**
 * Observer Pattern (Frontend): EventBus / Subject
 * Permite componentelor să se aboneze la evenimente globale (ex: Succes Plată).
 */
class EventBus {
    constructor() {
        this.listeners = {};
    }

    /**
     * Subscribe to an event
     * @param {string} eventType 
     * @param {Function} listener 
     */
    subscribe(eventType, listener) {
        if (!this.listeners[eventType]) {
            this.listeners[eventType] = [];
        }
        this.listeners[eventType].push(listener);
        
        // Return unsubscribe function
        return () => {
            this.listeners[eventType] = this.listeners[eventType].filter(l => l !== listener);
        };
    }

    /**
     * Notify all subscribers
     * @param {string} eventType 
     * @param {any} data 
     */
    notify(eventType, data) {
        if (!this.listeners[eventType]) return;
        
        console.log(`[EventBus] Notifying listeners for event: ${eventType}`, data);
        this.listeners[eventType].forEach(listener => listener(data));
    }
}

const globalEventBus = new EventBus();
export default globalEventBus;
