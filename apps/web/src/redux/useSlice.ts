import { createSlice, PayloadAction } from '@reduxjs/toolkit';

interface UserState {
    id: string | null;
    username: string | null;
    email: string | null;
}

const initialState: UserState = {
    id: localStorage.getItem('userId'),
    username: null,
    email: null,
};

const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        setUser(state, action: PayloadAction<{ id: string; username: string | null; email: string | null }>) {
            state.username = action.payload.username;
            state.email = action.payload.email;
            localStorage.setItem('userId', action.payload.id);
        },
        clearUser(state) {
            state.id = null;
            state.username = null;
            state.email = null;
            localStorage.removeItem('userId');
        },
    },
});

export const { setUser, clearUser } = userSlice.actions;
export default userSlice.reducer;