import actions from "./actions.js";
import mutations from "./mutations.js";

export default {
    namespaced: true,
    state() {
        return {
            identifiers: [],
            page: 0,
            pageSize: 5,
            endOfList: false
        }
    },
    getters: {
        identifiers(state) {
            return state.identifiers;
        },
        hasMore(state) {
            return !state.endOfList;
        }
    },
    actions,
    mutations
}