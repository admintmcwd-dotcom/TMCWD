// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class WebClient {

    url = "";
    data = null
    constructor(url, data) {
        this.url = url;
        this.data = data;
    }

    async postAsync() {
        var returnResult = null;
        await fetch(this.url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.data)
        }).then(response => {
            if (!response.ok)
                return null;
            return response.json();
        }).then(result => {
            returnResult = result;
        });
        
        return returnResult;
    }

}