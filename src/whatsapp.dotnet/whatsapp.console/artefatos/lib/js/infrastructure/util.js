
() => {

    window.Util = class Util {

        constructor() {
            throw new Error(`The ${this.constructor.name} class may not be instantiated.`);
        }


        static getQrCode = async () => {
            return new Promise(resolve => {
                let qrRetry =
                    document.querySelector("div[data-ref] > span > button");
                if (qrRetry) {
                    qrRetry.click();
                }

                const element = document.querySelectorAll('[data-ref]')[0];

                if (!element) {
                    resolve("");
                } else {

                    const qrCode = element.getAttribute('data-ref');
                    if (qrCode)
                        resolve(qrCode);
                    else
                        resolve("");
                }
            });
        }

        static getElementByXpath = (path) => {


            try {
                let element = document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null);

                if (element)
                    return element.singleNodeValue;
                return null;
            } catch (e) {
                return null;
            }
        }

        static buildData = (object) => {
            const jsonData = JSON.parse(object);
            for (var i = 0; i < jsonData.counters.length; i++) {
                var counter = jsonData.counters[i];

            }
        }

        static downloadFileWithCredentials = async (url) => {


            try {

                return new Promise(resolve => {
                    let xhr = new XMLHttpRequest();
                    xhr.onload = function () {
                        if (xhr.readyState == 4) {
                            if (xhr.status == 200) {
                                let reader = new FileReader();
                                reader.readAsDataURL(xhr.response);
                                reader.onload = function (e) {
                                    resolve(reader.result.substr(reader.result.indexOf(',') + 1))
                                };
                            } else {

                                resolve(xhr.statusText);
                            }
                        } else {
                            
                            resolve('err');
                        }
                    };

                    xhr.open("GET", url, true);
                    xhr.withCredentials = true;
                    xhr.responseType = 'blob';
                    xhr.send(null);
                    return null;
                });
            } catch (e) {
                return null;
            }
        }

        static serializeContactObj = (obj) => {
            if (obj == undefined) {
                return null;
            }

            return Object.assign(this.serializeRawObj(obj), {
                profilePicThumbObj: obj.profilePicThumb ? this.serializeProfilePicThumb(obj.profilePicThumb) : null
            });
        };

        static serializeRawObj = (obj) => {
            if (obj) {
                return obj.toJSON();
            }
            return {}
        };

        static serializeProfilePicThumb = (obj) => {
            if (obj == undefined) {
                return null;
            }

            return Object.assign({}, {
                eurl: obj.eurl,
                id: obj.id,
                img: obj.img,
                imgFull: obj.imgFull,
                raw: obj.raw,
                tag: obj.tag
            });
        }

        static getChatModelChange = (chat) => {

            let res = chat.serialize();

            if (chat['contact']) {

                let contact = this.serializeContactObj(chat['contact']);

                if (contact && contact.profilePicThumbObj) {

                    let profilePicThumbObj = contact.profilePicThumbObj;
                    res.eurl = profilePicThumbObj.eurl;
                    res.img = profilePicThumbObj.img;
                    res.imgFull = profilePicThumbObj.imgFull;
                }
            }

            res.formattedTitle = chat.formattedTitle;
            res.isGroup = chat.isGroup;
            res.formattedTitle = chat.formattedTitle;
            res.isMuted = chat.mute && chat.mute.isMuted;

            if (chat.groupMetadata) {
                res.groupMetadata = chat.groupMetadata.serialize();
            }

            delete res.msgs;
            delete res.msgUnsyncedButtonReplyMsgs;
            delete res.unsyncedButtonReplies;

            return res;
        };

    };
}