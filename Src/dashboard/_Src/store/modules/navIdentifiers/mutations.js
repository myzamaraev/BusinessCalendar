export default {
  nextPage(state) {
    state.page++;
  },
  endOfList(state) {
    state.endOfList = true;
  },
  reset(state) {
    state.page = 0,
    state.endOfList = false;
    state.identifiers = []
  },
  setData(state, data) {
    state.identifiers = data;
  },
  appendData(state, data) {
    state.identifiers.push.apply(state.identifiers, data);
  },
};
