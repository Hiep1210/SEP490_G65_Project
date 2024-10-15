/* eslint-disable @typescript-eslint/no-unused-vars */
import type { UseFetchOptions } from "#app";

export const useFetchApi = (url: string, options?: UseFetchOptions<object>) => {
    return useFetch(url, {
        ...options,
        async onRequest({ request, options }) {
            const headers = new Headers(options.headers);
            headers.set("Authorization", `Bearer ${document.cookie.split('; ').find((row) => row.startsWith('access_token'))?.split('=')[1] || null}`);
            options.headers = headers;
            this.credentials = "include";
        },
        async onRequestError({ request, options, error }) {
            console.error(error);
        },
        async onResponse({ request, response, options }) {
        },
        async onResponseError({ request, response, options }) {
            if (response.status === 401) {
                if(confirm("Your session has expired. Please login again.")){
                    useAuth().logout();
                };
            }
        },
    });
};