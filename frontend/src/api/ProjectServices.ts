import axios from "@/plugins/axios";


export interface IProjectCreate {
    name: string
}

export interface IProject {
    id: number,
    name: string,
    createdAt: string,
    updatedAt: string,
    taskCount: number
}

export const getAllProject = async () => {
    const response = await axios.get('/Project')
    return response.data
}

export const createProject = async (project: IProjectCreate) => {
    const response = await axios.post('/Project', project)
    return response.data
}

export const getProjectById = async (id : number) => {
    const response = await axios.get(`/Project/${id}`)
    return response.data
}

export const deleteProject = async (id : number) => {
    const response = await axios.delete(`/Project/${id}`)
    return response.data
}