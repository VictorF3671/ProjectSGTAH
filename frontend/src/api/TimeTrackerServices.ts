// src/api/TimeTrackerServices.ts
import axios from "@/plugins/axios"

export interface ITimeTrackerCreate {
  taskId: number
  collaboratorId: number
  startDate: string   // ISO-8601 UTC string
  endDate: string     // ISO-8601 UTC string
  timeZoneId: string
}

export interface ITimeTracker {
  id: number
  taskId: number
  collaboratorId: number
  startDate: string     // ISO-8601 UTC
  endDate: string       // ISO-8601 UTC
  timeZoneId: string
  createdAt: string     // ISO-8601 UTC
  updatedAt: string     // ISO-8601 UTC
  durationHours: number 
}

export const getAllTimeTrackers = async (
  taskId?: number,
  collaboratorId?: number
): Promise<ITimeTracker[]> => {
  const response = await axios.get<ITimeTracker[]>("/TimeTracker", {
    params: { taskId, collaboratorId },
  })
  return response.data
}

export const getTimeTrackerById = async (id: number): Promise<ITimeTracker> => {
  const response = await axios.get<ITimeTracker>(`/TimeTracker/${id}`)
  return response.data
}

export const createTimeTracker = async (
  entry: ITimeTrackerCreate
): Promise<ITimeTracker> => {
  const response = await axios.post<ITimeTracker>("/TimeTracker", entry)
  return response.data
}

export const deleteTimeTracker = async (id: number): Promise<void> => {
  await axios.delete(`/TimeTracker/${id}`)
}

export const getTodayTotal = async (
  collaboratorId: number
): Promise<string> => {
  const response = await axios.get<string>(`/TimeTracker/today/${collaboratorId}`)
  return response.data
}

export const getMonthTotal = async (
  collaboratorId: number
): Promise<string> => {
  const response = await axios.get<string>(`/TimeTracker/month/${collaboratorId}`)
  return response.data
}