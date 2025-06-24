import axios from '@/plugins/axios'; 

export interface IUser {
    id: number,
    username: string,
    createdAt: Date,
    updatedAt: Date
}

export interface ILogin {
    username: string,
    password: string
}
export interface IUserCreate {
    username: string,
    password: string
}

export const authUser = async (login: ILogin) => {
    const response = await axios.post('/User/Login', login)
    return response.data;
}

export const getAllUsers = async () => {
    const response = await axios.get('/Users')
    return response.data;
}

export const getUserById = async (id: number) => {
    const response = await axios.get(`/User/${id}`)
    return response.data;
}

export const createUser = async (user: IUserCreate) =>{
    const response = await axios.post('/User', user)
    return response.data;
}
