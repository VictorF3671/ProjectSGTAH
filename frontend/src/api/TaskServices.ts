import axios from "@/plugins/axios"

export interface ITaskCreate {
    name: string,
    description: string,
    projectId: number

}

export interface ITask {
    id: number,
    name: string,
    description: string,
    projectId: number,
    createdAt: string,
    updatedAt: string,
    timeEntries: number
}

export const getAllTask = async (projectId: number) => {
    const response = await axios.get(`/Task/${projectId}`)
    return response.data
}

export const createTask = async (project: ITaskCreate) => {
    const response = await axios.post('/Task', project)
    return response.data
}

export const getTaskById = async (id : number) => {
    const response = await axios.get(`/Task/${id}`)
    return response.data
}

export const updateTask = async (id : number) => {
    const response = await axios.put(`/Task/${id}`)
    return response.data
}

export const deleteTask = async (id : number) => {
    const response = await axios.delete(`/Task/${id}`)
    return response.data
}