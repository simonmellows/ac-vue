import { ref } from "vue"

function createFileUrl(domain, filePath, secure, port){
    return `http${secure ? 's' : ''}://${domain}${port ? ':' : ''}${port || ''}/${filePath}`
}

export function useFetchFile()
{   
    const file = ref(null)
    const error = ref(null)
    const fetching = ref(false)

    async function fetchFile(url, timeout = 5000){
        // Function to fetch file
        file.value = null
        error.value = null

        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), timeout);

        console.log("Fetching file from: ", url)
        fetching.value = true

        try {
            const response = await fetch(url, { signal: controller.signal });
            clearTimeout(timeoutId);
            fetching.value = false
            
            if (!response.ok) {
                throw new Error(`Unable to fetch file from URL: ${url}`);
            }
        
            file.value = response;

        } catch (err) {
            fetching.value = false
            error.value = err
        }
    }

    return {
        fetchFile,
        createFileUrl,
        fetching,
        error,
        file
    }
}