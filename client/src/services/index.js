const baseURL = import.meta.env.VITE_API_URL;

const fetchData = ({url, headers = {}, method = 'GET', data = null}) =>
    fetch(url, {
        method,
        headers: {
            ...headers,
        },
        body: data ? JSON.stringify(data) : null,
    }).then(response => response);

const handleResponseResult = async response => {
    if (!response.headers.get('Content-Type')?.startsWith('application/json')) {
        return {
            message: 'The response is not JSON',
            status: response.status,
            hasError: !response.ok,
        };
    }
    const {errors, errorCode, message, result} = await response.json();
    return {
        status: response.status,
        hasError: !response.ok,
        errors,
        errorCode,
        message,
        result,
    };
};

const getConvertedDocumentsService = async () => {
    try {
        const response = await fetchData({
            url: baseURL,
        });

        return handleResponseResult(response);
    } catch (error) {
        return {
            hasError: true,
            message: error.message,
        };
    }
};

const deleteConvertedDocumentsService = async id => {
    try {
        const response = await fetchData({
            url: `${baseURL}/${id}`,
            method: 'DELETE',
        });

        return handleResponseResult(response);
    } catch (error) {
        return {
            hasError: true,
            message: error.message,
        };
    }
};

const downloadDocumentService = async data => {
    try {
        const response = await fetch(baseURL, {
            method: 'POST',
            body: data,
        });
        return handleResponseResult(response);
    } catch (error) {
        return {
            hasError: true,
            message: error.message,
        };
    }
};

export {
    getConvertedDocumentsService,
    deleteConvertedDocumentsService,
    downloadDocumentService,
};
