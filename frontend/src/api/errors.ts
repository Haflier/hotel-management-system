import axios from "axios";

export function getApiErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error)) {
    const data = error.response?.data;

    if (typeof data === "string" && data.trim()) {
      return data;
    }

    if (data?.message) {
      return data.message;
    }

    if (data?.title) {
      return data.title;
    }

    if (error.response?.status === 400) {
      return "The request was invalid.";
    }

    if (error.response?.status === 401) {
      return "You are not authorized. Please log in.";
    }

    if (error.response?.status === 403) {
      return "You do not have permission to perform this action.";
    }

    if (error.response?.status === 404) {
      return "The requested resource was not found.";
    }

    if (error.response?.status === 409) {
      return "The request conflicts with existing data.";
    }

    if (error.response?.status && error.response.status >= 500) {
      return "The server encountered an error.";
    }

    if (error.code === "ECONNABORTED") {
      return "The request timed out.";
    }

    if (!error.response) {
      return "Could not connect to the API.";
    }

    return `Request failed with status ${error.response.status}.`;
  }

  if (error instanceof Error) {
    return error.message;
  }

  return "An unexpected error occurred.";
}
