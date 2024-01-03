/**
* A regular array with basic subscription capabilities.
* NOTE: This isn't fully implemented, so if used in the future you
* may need to add additional method overrides to call the subscribers.
*/
export class ObservableArray extends Array {

    _subscribers = [];

    _raiseChangeEvent() {
        this._subscribers.forEach((subscriberCallback) => {
            subscriberCallback();
        });
    }

    /**
     * Adds a subscriber callback to this array.
     * */
    subscribe(subscriberCallback) {
        this._subscribers.push(subscriberCallback);
    }

    /**
     * Removes a subscriber callback from this array.
     * */
    unsubscribe(subscriberCallback) {
        const indexOfSubscriber = this._subscribers.indexOf(subscriberCallback);
        this._subscribers = this._subscribers.splice(indexOfSubscriber, 1);
    }

    /**
     * Adds an item to this array and calls any subscribers.
     * */
    push(item) {
        super.push(item);
        this._raiseChangeEvent();
    }

    /**
     * Adds multiple items to this array. The subscriber callback
     * is only called once after all of the items have been added.
     * */
    pushMany(items) {
        items.forEach(x => {
            super.push(x);
        });
        this._raiseChangeEvent();
    }
}