import axios from "@/plugins/axios"

export interface ICollaboratorCreate {
  userId: number
}

export interface ICollaborator {
  id: number
  userId: number
  userName: string
}

export const getAllCollaborators = async (): Promise<ICollaborator[]> => {
  const response = await axios.get<ICollaborator[]>("/Collaborator")
  return response.data
}

export const getCollaboratorById = async (
  id: number
): Promise<ICollaborator> => {
  const response = await axios.get<ICollaborator>(`/Collaborator/${id}`)
  return response.data
}

export const createCollaborator = async (
  dto: ICollaboratorCreate
): Promise<ICollaborator> => {
  const response = await axios.post<ICollaborator>("/Collaborator", dto)
  return response.data
}

export const updateCollaborator = async (
  id: number,
  dto: ICollaboratorCreate
): Promise<ICollaborator> => {
  const response = await axios.put<ICollaborator>(`/Collaborator/${id}`, dto)
  return response.data
}

export const deleteCollaborator = async (id: number): Promise<void> => {
  await axios.delete(`/Collaborator/${id}`)
}