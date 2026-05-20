// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

HTMLElement.prototype.setDropdown = function (selectCallBack) {
    const elSelect = this;
    var isListenersAttached = false;

    if (elSelect.tagName.toLowerCase() === "select") {
        
        const parentDiv = elSelect.parentElement;
        var btnCollapse = null;
        var icon = null;
        var elContent = null;
        var elOptions = null;

        if (parentDiv) {
            btnCollapse = parentDiv.querySelector("button");
            if (btnCollapse) {
                btnCollapse.addEventListener("click", (evt) => {
                    evt.preventDefault();
                    icon = btnCollapse.querySelector("i");
                    elContent = parentDiv.getElementsByClassName("dropdown-options");
                    icon.classList.toggle('fa-chevron-down');
                    icon.classList.toggle('fa-chevron-up');
                    elContent[0].classList.toggle("hidden");

                    const inputs = parentDiv.getElementsByClassName("dropdown-input");

                    if (isListenersAttached) return;

                    var contents = elContent[0].children;
                    var elOptions = [];

                    [...contents].forEach((element) => {
                        elOptions.push(element);
                    });

                    var links = elContent[0].querySelectorAll('a');
                    if (links) {

                        [...links].forEach((link) => {
                            link.addEventListener("click", (evt) => {
                                evt.preventDefault();
                                evt.stopPropagation();
                                
                                const elSelectedValues = parentDiv.getElementsByClassName("selected-value");
                                var itemId = evt.target.parentElement.dataset.itemid;
                                var itemName = evt.target.parentElement.dataset.itemname;
                                if (inputs) inputs[0].value = itemName;
                                if (elSelectedValues) elSelectedValues[0].value = itemId;
                                if (selectCallBack) selectCallBack(itemId, itemName);
                                icon.classList.toggle('fa-chevron-down');
                                icon.classList.toggle('fa-chevron-up');
                                elContent[0].classList.toggle("hidden");
                            });
                        });
                    }


                    if (inputs) {
                        inputs[0].addEventListener("input", (evt) => {
                            evt.preventDefault();
                            evt.stopPropagation();
                            var searchString = evt.target.value.toLowerCase();
                            const elFiltered = [...elOptions].filter((element, index, array) => {
                                var parContent = element.querySelector("p");
                                return parContent.textContent.toLowerCase().includes(searchString.toLowerCase());
                            });
                            elContent[0].replaceChildren();

                            var result = searchString.trim() == '' ? elOptions : elFiltered;
                            [...result].forEach((el) => {
                                elContent[0].appendChild(el);
                            });
                        });
                    }

                    isListenersAttached = true;

                });
            }
        }
    }
};

class WebClient {

    url = "";
    data = null;
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

    async getAsync() {
        var returnResult = null;
        await fetch(this.url, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(response => {
            if (!response.ok)
                return null;
            return response.json();
        }).then(result => {
            returnResult = result;
        });

        return returnResult;
    }

};

class ModalDialog {

    dialog = null;
    cancelButton = null;
    submitButton = null;
    inputText = null;
    fnSubmitCallback = null;
    title = "";
    content = "";
    textTitle = "";

    constructor(params) {
        this.title = params.title;
        this.content = params.content;
        this.textTitle = params.textTile;
        this.fnSubmitCallback = params.submitCallback;
        this.#initialize(params.type);
    }

    show() {
        this.dialog.showModal();

        this.#cancelAddListener();
        this.#submitAddListener();
    }

    close() {
        this.dialog.close();
    }

    #initialize(type) {
        this.#setTitleAndContent(type);
        this.#setButton(type);
    }

    #setTitleAndContent(type) {
        var elTitle = null;
        var elContent = null;
        var elTextTitle = null;
        switch (type) {
            case 'deactivate':
                elTitle = document.getElementsByClassName("deactivate-title");
                elContent = document.getElementsByClassName("deactivate-content");
                break;
            case 'success':
                elTitle = document.getElementsByClassName("success-title");
                elContent = document.getElementsByClassName("success-content");
                break;
            case 'number':
                elTitle = document.getElementsByClassName("number-title");
                elContent = document.getElementsByClassName("number-content");
                elTextTitle = document.getElementsByClassName("number-text-title");
                break;
            case "text":
                elTitle = document.getElementsByClassName("text-title");
                elContent = document.getElementsByClassName("text-content");
                elTextTitle = document.getElementsByClassName("text-text-title");
                break;
            default:
                break;
        }

        if (elTitle) elTitle[0].innertText = this.title;
        if (elContent) elContent[0].innertText = this.content;
        if (elTextTitle) elTextTitle[0].innerText = this.textTitle;
    }

    #setButton(type) {
        var dialogClassName = ""
        var cancelButtonClassName = "";
        var submitButtonClassName = "";
        var inputClassName = ""
        switch (type) {
            case 'deactivate':
                dialogClassName = "deactivate-modal";
                cancelButtonClassName = "deactivate-cancel"
                submitButtonClassName = "deactivate-submit";
                break;
            case 'success':
                dialogClassName = "success-modal";
                cancelButtonClassName = "success-cancel"
                submitButtonClassName = "success-submit";
                break;
            case 'number':
                dialogClassName = "number-modal";
                cancelButtonClassName = "number-cancel"
                submitButtonClassName = "number-submit";
                inputClassName = "number-text";
                break;
            case 'text':
                dialogClassName = "text-modal";
                cancelButtonClassName = "text-cancel";
                submitButtonClassName = "text-submit";
                inputClassName = "text-text";
                break;
            default:
                dialogClassName = "";
                break;
        }
        var elDialog = document.getElementsByClassName(dialogClassName);
        var elCancel = document.getElementsByClassName(cancelButtonClassName);
        var elSubmit = document.getElementsByClassName(submitButtonClassName);
        var elText = document.getElementsByClassName(inputNumberClassName);
        if (elDialog) this.dialog = elDialog[0];
        if (elCancel) this.cancelButton = elCancel[0];
        if (elSubmit) this.submitButton = elSubmit[0];
        if (elText) this.inputText = elText[0];
    }

    #cancelAddListener() {
        if (this.cancelButton == null) return;
        this.cancelButton.addEventListener("click", (evt) => {
            evt.preventDefault();
            this.dialog.close();
        }, { once: true });
    }

    #submitAddListener() {
        if (this.submitButton == null) return;
        this.submitButton.addEventListener("click", (evt) => {
            evt.preventDefault();
            if (this.fnSubmitCallback) {
                if (this.numberText == null) {
                    this.fnSubmitCallback();
                }
                else {
                    var textValue = this.numberText.value;
                    this.fnSubmitCallback(textValue);
                    this.dialog.close();
                }
            }
        }, { once: true });
    }

};

class Alert {
    constructor() { }

    show(options) {
        if (options) {
            const alertWindow = document.getElementById(options.type + 'Alert');
            var alertSpan = document.querySelector('#' + options.type + 'Alert span');
            alertSpan.innerText = options.content;
            alertWindow.classList.remove('hidden');
            var timeOutInSeconds = options.timeOut ?? 3000;
            if (options.autoHide) {
                setTimeout((elem) => {
                    elem.classList.add('hidden');
                }, timeOutInSeconds, alertWindow);
            }
        }
    }

};

class CustomerSelect {
    getCustomerUrl = '';
    fnCallback = null;
    btnCancel = null;
    btnSubmit = null;
    btnSearch = null;
    txtSearch = null;
    tbodyList = null;
    dialog = null;

    constructor(params) {
        this.getCustomerUrl = params.getCustomerUrl;
        this.fnCallback = params.selectCallback;
        this.#initialize();
    }

    show() {
        //search code
        if (this.btnSearch) {
            this.btnSearch.addEventListener("click", (evt) => {
                evt.preventDefault();
                this.#seach();
            }, { once: true });
        }

            //submit code
        if (this.btnSubmit) {
            this.btnSubmit.addEventListener("click", (evt) => {
                evt.preventDefault();

                this.#select();

                this.dialog.close();

            }, { once: true });
        }

        //cancel code
        if (this.btnCancel) {
            this.btnCancel.addEventListener("click", (evt) => {
                evt.preventDefault();
                this.dialog.close();
            }, { once: true });
        }

        if (this.dialog) this.dialog.showModal();
    }

    #initialize() {
        var dialogs = document.getElementsByClassName("customer-select-dialog");
        var cancelButtons = document.getElementsByClassName("customer-select-cancel");
        var submitButtons = document.getElementsByClassName("customer-select-submit");
        var searchButtons = document.getElementsByClassName("customer-select-search");
        var inputs = document.getElementsByClassName("customer-select-customer-name");
        var tbody = document.querySelector(".customer-select-list tbody");

        if (dialogs) this.dialog = dialogs[0];
        if (cancelButtons) this.btnCancel = cancelButtons[0];
        if (submitButtons) this.btnSubmit = submitButtons[0];
        if (searchButtons) this.btnSearch = searchButtons[0];
        if (inputs) this.txtSearch = inputs[0];
        if (tbody) this.tbodyList = tbody;
    }

    #seach = async () => {
        if (this.txtSearch) {
            var searchString = encodeURIComponent(this.txtSearch.value);
            var searchUrl = this.getCustomerUrl + '?searchString=' + searchString;
            var client = new WebClient(searchUrl, null);
            var result = await client.getAsync();
            if (result) {
                if (this.tbodyList) {
                    this.tbodyList.replaceChildren();

                    for (var ctr = 0; ctr < result.length; ctr++) {
                        var customer = result[ctr];
                        var tr = document.createElement("tr");
                        tr.classList.add("bg-white", "border-b", "hover:bg-gray-50");
                        tr.innerHTML = `
                        <td class="px-6 py-4 text-center">
                            <input data-email="${customer.email}" data-phone="${customer.phoneNumber}" data-customerid="${customer.id}" data-customername="${customer.lastname + ', ' + customer.firstname + ' ' + customer.middlename}" type="radio" name="customerSelect" value="${customer.id}">
                        </td>
                        <td class="px-6 py-4">${customer.firstname} ${customer.lastname}</td>
                        <td class="px-6 py-4">${customer.email}</td>
                        <td class="px-6 py-4">${customer.phoneNumber}</td>
                        <td class="px-6 py-4 text-center">
                            ${customer.isActive ? '<i class="fa-solid fa-check text-green-500"></i>' : '<i class="fa-solid fa-xmark text-red-500"></i>'}
                        </td>
                    `;
                        this.tbodyList.appendChild(tr);
                    }
                }
            }
        }
    }

    #select() {
        var selectedRadio = this.dialog.querySelectorAll('input[type="radio"]:checked');
        var customerId = selectedRadio[0].dataset.customerid;
        var phone = encodeURIComponent(selectedRadio[0].dataset.phone);
        var email = encodeURIComponent(selectedRadio[0].dataset.email);
        var customerName = encodeURIComponent(selectedRadio[0].dataset.customername);
        this.fnCallback(customerId, customerName, phone, email);
    }
};

class AccountSelect{
    //getAccountUrl = '';
    addUrl = '';
    fnCallback = null;
    btnCancel = null;
    btnSubmit = null;
    btnAdd = null;
    txtAdd = null;
    tbody = null;
    dialog = null;
    customerId = 0;
    constructor(params) {
        if (params) {
            if (!params.customerid || params.customerid <= 0) throw new Error("CustomerId is required to initialize AccountSelect");
            if (params.addAccountUrl) this.addUrl = params.addAccountUrl;
            if (params.submitCallback) this.fnCallback = params.submitCallback;
            //if (params.getAccountUrl) this.getAccountUrl = params.getAccountUrl;
            this.customerId = params.customerid;
        }
        this.#initialize();
    }

    show() {
        if (this.dialog) {
            this.dialog.showModal();
            this.#setAddButtonListener();
            this.#setSubmitButtonListener();
            this.#setCancelButtonListener();
        }
    }

    #initialize(){
        const elDialogs = document.getElementsByClassName("account-select-dialog");
        const elInputs = document.getElementsByClassName("account-select-account-address");
        const elAddButtons = document.getElementsByClassName("account-select-add");
        const elTableBody = document.querySelector(".account-select-list tbody");
        const elSubmitButtons = document.getElementsByClassName("account-select-submit");
        const elCancelButtons = document.getElementsByClassName("account-select-cancel");

        if (elDialogs) this.dialog = elDialogs[0];
        if (elInputs) this.txtAdd = elInputs[0];
        if (elAddButtons) this.btnAdd = elAddButtons[0];
        if (elTableBody) this.tbody = elTableBody;
        if (elSubmitButtons) this.btnSubmit = elSubmitButtons[0];
        if (elCancelButtons) this.btnCancel = elCancelButtons[0];
    }

    #setAddButtonListener() {
        if (this.btnAdd && this.addUrl != '') {
            if (!this.txtAdd) throw new Error('Text input for adding account is required');
            this.btnAdd.addEventListener("click", async (evt) => {
                evt.preventDefault();
                var accountAddress = encodeURIComponent(this.txtAdd.value.trim());
                if (this.txtAdd.value.trim() == '') throw new Error('Account address is required');
                var addClient = new WebClient(this.addUrl, { customerId: this.customerId, accountAddress: accountAddress });
                var account = await addClient.postAsync();
                if (account) {
                    console.log('Saved Account:', account);
                    this.#refreshTable(account);
                }
                else {
                    console.log('Account was not added');
                }

            }, { once: true });
        }
    }

    #setSubmitButtonListener() {
        if (this.btnSubmit && this.dialog) {
            this.btnSubmit.addEventListener("click", (evt) => {
                evt.preventDefault();
                var selectedRadio = this.dialog.querySelectorAll('input[type="radio"]:checked');
                if (selectedRadio) {
                    var accountId = selectedRadio[0].dataset.id;
                    var accountNumber = encodeURIComponent(selectedRadio[0].dataset.accountno);
                    var address = selectedRadio[0].dataset.address;
                    this.fnCallback(accountId, accountNumber, address);
                    this.dialog.close();
                }
            }, { once: true });
        }
    }

    #setCancelButtonListener() {
        if (this.btnCancel) {
            if (!this.fnCallback) throw new Error('Submit callback is required for Account Select');
            this.btnCancel.addEventListener("click", (evt) => {
                evt.preventDefault();
                
                if (this.dialog) this.dialog.close();
            }, { once: true });
        }
    }

    #refreshTable(account) {
        if (this.tbody) {
            //this.tbody.replaceChildren();

            var tr = document.createElement("tr");
            tr.classList.add("bg-white", "border-b", "hover:bg-gray-50");
            //<td class="px-6 py-4 text-center">${this.#loadAccountStatusContent(+ account.status)}</td>
            tr.innerHTML = `
                    <td class="px-6 py-4 text-center">
                        <input data-id="{account.id}" data-accountno="${account.accountNumber}" data-address="${encodeURIComponent(account.address)}" data-current="${account.isCurrentAddress}" type="radio" name="customerSelect" value="${account.id}">
                    </td>
                    <td class="px-6 py-4">${account.accountNumber}</td>
                    <td class="px-6 py-4">${account.address}</td>
                    <td class="px-6 py-4">
                        ${account.isCurrentAddress ? '<i class="fa-solid fa-check text-green-500"></i>' : ''}
                    </td>
                `;
            tr.appendChild(this.#addAccountStatusColumn(account.status));
            this.tbody.appendChild(tr);
        }
    }

    #addAccountStatusColumn(status) {
        var icon = document.createElement("i");
        var td = document.createElement("td");
        td.classList.add("px-6", "py-4");
        switch (status) {
            case 1:
                break;
                icon.classList.add("fa-solid", "fa-question", "text-yellow-500");
            case 2:
                icon.classList.add("fa-solid", "fa-pause", "text-gray-500");
                break;
            case 3:
                icon.classList.add("fa-solid", "fa-check", "text-green-500");
                break;
            default:
                icon.classList.add("fa-solid", "fa-xmark", "text-red-500");
                break;
        }
        td.appendChild(icon);
        return td;
    }

};