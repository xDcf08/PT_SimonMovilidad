import { apiClient } from "./apiClient";


export const authService = {
  login: async (email: string, password: string): Promise<string> => {
    return apiClient.post("users/login", {email, password});
  }
}