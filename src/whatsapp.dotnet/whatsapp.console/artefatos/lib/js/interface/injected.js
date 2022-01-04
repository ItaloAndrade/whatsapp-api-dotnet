() => {

    window.moduleRaid = function () {
        moduleRaid.mID = Math.random().toString(36).substring(7);
        moduleRaid.mObj = {};

        fillModuleArray = function () {
            (window.webpackChunkbuild || window.webpackChunkwhatsapp_web_client).push([
                [moduleRaid.mID], {}, function (e) {
                    Object.keys(e.m).forEach(function (mod) {
                        moduleRaid.mObj[mod] = e(mod);
                    })
                }
            ]);
        }

        fillModuleArray();

        get = function get(id) {
            return moduleRaid.mObj[id]
        }

        findModule = function findModule(query) {
            results = [];
            modules = Object.keys(moduleRaid.mObj);

            modules.forEach(function (mKey) {
                mod = moduleRaid.mObj[mKey];

                if (typeof mod !== 'undefined') {
                    if (typeof query === 'string') {
                        if (typeof mod.default === 'object') {
                            for (key in mod.default) {
                                if (key == query) results.push(mod);
                            }
                        }

                        for (key in mod) {
                            if (key == query) results.push(mod);
                        }
                    } else if (typeof query === 'function') {
                        if (query(mod)) {
                            results.push(mod);
                        }
                    } else {
                        throw new TypeError('findModule can only find via string and function, ' + (typeof query) + ' was passed');
                    }

                }
            })

            return results;
        }

        return {
            modules: moduleRaid.mObj,
            constructors: moduleRaid.cArr,
            findModule: findModule,
            get: get
        }
    };
    window.mR = moduleRaid();
    window.Store = Object.assign({}, window.mR.findModule('Chat')[0].default);
    window.Store.AppState = window.mR.findModule('STREAM')[0].Socket;
    window.Store.Conn = window.mR.findModule('Conn')[0].Conn;
    window.Store.CryptoLib = window.mR.findModule('decryptE2EMedia')[0];
    window.Store.Wap = window.mR.findModule('queryLinkPreview')[0].default;
    window.Store.SendSeen = window.mR.findModule('sendSeen')[0];
    window.Store.SendClear = window.mR.findModule('sendClear')[0];
    window.Store.SendDelete = window.mR.findModule('sendDelete')[0];
    window.Store.genId = window.mR.findModule('randomId')[0].randomId;
    window.Store.SendMessage = window.mR.findModule('addAndSendMsgToChat')[0];
    window.Store.MsgKey = window.mR.findModule((module) => module.default && module.default.fromString)[0].default;
    window.Store.Invite = window.mR.findModule('sendJoinGroupViaInvite')[0];
    window.Store.OpaqueData = window.mR.findModule(module => module.default && module.default.createFromData)[0].default;
    window.Store.MediaPrep = window.mR.findModule('MediaPrep')[0];
    window.Store.MediaObject = window.mR.findModule('getOrCreateMediaObject')[0];
    window.Store.MediaUpload = window.mR.findModule('uploadMedia')[0];
    window.Store.NumberInfo = window.mR.findModule('formattedPhoneNumber')[0];
    window.Store.Cmd = window.mR.findModule('Cmd')[0].Cmd;
    window.Store.MediaTypes = window.mR.findModule('msgToMediaType')[0];
    window.Store.VCard = window.mR.findModule('vcardFromContactModel')[0];
    window.Store.UserConstructor = window.mR.findModule((module) => (module.default && module.default.prototype && module.default.prototype.isServer && module.default.prototype.isUser) ? module.default : null)[0].default;
    window.Store.Validators = window.mR.findModule('findLinks')[0];
    window.Store.WidFactory = window.mR.findModule('createWid')[0];
    window.Store.BlockContact = window.mR.findModule('blockContact')[0];
    window.Store.GroupMetadata = window.mR.findModule((module) => module.default && module.default.handlePendingInvite)[0].default;
    window.Store.Sticker = window.mR.findModule('Sticker')[0].default.Sticker;
    window.Store.UploadUtils = window.mR.findModule((module) => (module.default && module.default.encryptAndUpload) ? module.default : null)[0].default;
    window.Store.Label = window.mR.findModule('LabelCollection')[0].default;
    window.Store.Features = window.mR.findModule('FEATURE_CHANGE_EVENT')[0].GK;
    window.Store.QueryOrder = window.mR.findModule('queryOrder')[0];
    window.Store.QueryProduct = window.mR.findModule('queryProduct')[0];
    window.Store.DownloadManager = window.mR.findModule('downloadManager')[0].downloadManager;
    window.Store.Call = window.mR.findModule('CallCollection')[0].CallCollection;


    class WWebJS {

        constructor() {
        }


        getNumberId = async (id) => {

            let result = await window.Store.Wap.queryExist(id);
            if (result.jid === undefined)
                throw 'The number provided is not a registered whatsapp user';
            return result.jid;
        };

        sendSeen = async (chatId) => {
            let chat = window.Store.Chat.get(chatId);
            if (chat !== undefined) {
                await window.Store.SendSeen.sendSeen(chat, false);
                return true;
            }
            return false;

        };

        sendMessage = async (chat, content, options = {}) => {
            debugger; 
            let attOptions = {};
            if (options.attachment) {
                attOptions = options.sendMediaAsSticker
                    ? await window.WWebJS.processStickerData(options.attachment)
                    : await window.WWebJS.processMediaData(options.attachment, {
                        forceVoice: options.sendAudioAsVoice,
                        forceDocument: options.sendMediaAsDocument,
                        forceGif: options.sendVideoAsGif
                    });

                content = options.sendMediaAsSticker ? undefined : attOptions.preview;

                delete options.attachment;
                delete options.sendMediaAsSticker;
            }

            let quotedMsgOptions = {};
            if (options.quotedMessageId) {
                let quotedMessage = window.Store.Msg.get(options.quotedMessageId);
                if (quotedMessage.canReply()) {
                    quotedMsgOptions = quotedMessage.msgContextInfo(chat);
                }
                delete options.quotedMessageId;
            }

            if (options.mentionedJidList) {
                options.mentionedJidList = options.mentionedJidList.map(cId => window.Store.Contact.get(cId).id);
            }

            let locationOptions = {};
            if (options.location) {
                locationOptions = {
                    type: 'location',
                    loc: options.location.description,
                    lat: options.location.latitude,
                    lng: options.location.longitude
                };
                delete options.location;
            }

            let vcardOptions = {};
            if (options.contactCard) {
                let contact = window.Store.Contact.get(options.contactCard);
                vcardOptions = {
                    body: window.Store.VCard.vcardFromContactModel(contact).vcard,
                    type: 'vcard',
                    vcardFormattedName: contact.formattedName
                };
                delete options.contactCard;
            } else if (options.contactCardList) {
                let contacts = options.contactCardList.map(c => window.Store.Contact.get(c));
                let vcards = contacts.map(c => window.Store.VCard.vcardFromContactModel(c));
                vcardOptions = {
                    type: 'multi_vcard',
                    vcardList: vcards,
                    body: undefined
                };
                delete options.contactCardList;
            } else if (options.parseVCards && typeof (content) === 'string' && content.startsWith('BEGIN:VCARD')) {
                delete options.parseVCards;
                try {
                    const parsed = window.Store.VCard.parseVcard(content);
                    if (parsed) {
                        vcardOptions = {
                            type: 'vcard',
                            vcardFormattedName: window.Store.VCard.vcardGetNameFromParsed(parsed)
                        };
                    }
                } catch (_) {
                    // not a vcard
                }
            }

            if (options.linkPreview) {
                delete options.linkPreview;
                const link = window.Store.Validators.findLink(content);
                if (link) {
                    const preview = await window.Store.Wap.queryLinkPreview(link.url);
                    preview.preview = true;
                    preview.subtype = 'url';
                    options = { ...options, ...preview };
                }
            }

            let extraOptions = {};
            if (options.buttons) {
                let caption;
                if (options.buttons.type === 'chat') {
                    content = options.buttons.body;
                    caption = content;
                } else {
                    caption = options.caption ? options.caption : ' '; //Caption can't be empty
                }
                extraOptions = {
                    productHeaderImageRejected: false,
                    isFromTemplate: false,
                    isDynamicReplyButtonsMsg: true,
                    title: options.buttons.title ? options.buttons.title : undefined,
                    footer: options.buttons.footer ? options.buttons.footer : undefined,
                    dynamicReplyButtons: options.buttons.buttons,
                    replyButtons: options.buttons.buttons,
                    caption: caption
                };
                delete options.buttons;
            }

            if (options.list) {
                if (window.Store.Conn.platform === 'smba' || window.Store.Conn.platform === 'smbi') {
                    throw '[LT01] Whatsapp business can\'t send this yet';
                }
                extraOptions = {
                    ...extraOptions,
                    type: 'list',
                    footer: options.list.footer,
                    list: {
                        ...options.list,
                        listType: 1
                    },
                    body: options.list.description
                };
                delete options.list;
                delete extraOptions.list.footer;
            }

            const newMsgId = new window.Store.MsgKey({
                fromMe: true,
                remote: chat.id,
                id: window.Store.genId(),
            });

            const message = {
                ...options,
                id: newMsgId,
                ack: 0,
                body: content,
                from: window.Store.Conn.wid,
                to: chat.id,
                local: true,
                self: 'out',
                t: parseInt(new Date().getTime() / 1000),
                isNewMsg: true,
                type: 'chat',
                ...locationOptions,
                ...attOptions,
                ...quotedMsgOptions,
                ...vcardOptions,
                ...extraOptions
            };

            await window.Store.SendMessage.addAndSendMsgToChat(chat, message);
            return window.Store.Msg.get(newMsgId._serialized);
        };

        processStickerData = async (mediaInfo) => {
            if (mediaInfo.mimetype !== 'image/webp') throw new Error('Invalid media type');

            const file = window.WWebJS.mediaInfoToFile(mediaInfo);
            let filehash = await window.WWebJS.getFileHash(file);
            let mediaKey = await window.WWebJS.generateHash(32);

            const controller = new AbortController();
            const uploadedInfo = await window.Store.UploadUtils.encryptAndUpload({
                blob: file,
                type: 'sticker',
                signal: controller.signal,
                mediaKey
            });

            const stickerInfo = {
                ...uploadedInfo,
                clientUrl: uploadedInfo.url,
                deprecatedMms3Url: uploadedInfo.url,
                uploadhash: uploadedInfo.encFilehash,
                size: file.size,
                type: 'sticker',
                filehash
            };

            return stickerInfo;
        };

        processMediaData = async (mediaInfo, { forceVoice, forceDocument, forceGif }) => {
            const file = window.WWebJS.mediaInfoToFile(mediaInfo);
            const mData = await window.Store.OpaqueData.createFromData(file, file.type);
            const mediaPrep = window.Store.MediaPrep.prepRawMedia(mData, { asDocument: forceDocument });
            const mediaData = await mediaPrep.waitForPrep();
            const mediaObject = window.Store.MediaObject.getOrCreateMediaObject(mediaData.filehash);

            const mediaType = window.Store.MediaTypes.msgToMediaType({
                type: mediaData.type,
                isGif: mediaData.isGif
            });

            if (forceVoice && mediaData.type === 'audio') {
                mediaData.type = 'ptt';
            }

            if (forceGif && mediaData.type === 'video') {
                mediaData.isGif = true;
            }

            if (forceDocument) {
                mediaData.type = 'document';
            }

            if (!(mediaData.mediaBlob instanceof window.Store.OpaqueData)) {
                mediaData.mediaBlob = await window.Store.OpaqueData.createFromData(mediaData.mediaBlob, mediaData.mediaBlob.type);
            }

            mediaData.renderableUrl = mediaData.mediaBlob.url();
            mediaObject.consolidate(mediaData.toJSON());
            mediaData.mediaBlob.autorelease();

            const uploadedMedia = await window.Store.MediaUpload.uploadMedia({
                mimetype: mediaData.mimetype,
                mediaObject,
                mediaType
            });

            const mediaEntry = uploadedMedia.mediaEntry;
            if (!mediaEntry) {
                throw new Error('upload failed: media entry was not created');
            }

            mediaData.set({
                clientUrl: mediaEntry.mmsUrl,
                deprecatedMms3Url: mediaEntry.deprecatedMms3Url,
                directPath: mediaEntry.directPath,
                mediaKey: mediaEntry.mediaKey,
                mediaKeyTimestamp: mediaEntry.mediaKeyTimestamp,
                filehash: mediaObject.filehash,
                encFilehash: mediaEntry.encFilehash,
                uploadhash: mediaEntry.uploadHash,
                size: mediaObject.size,
                streamingSidecar: mediaEntry.sidecar,
                firstFrameSidecar: mediaEntry.firstFrameSidecar
            });

            return mediaData;
        };

        getMessageModel = message => {
            const msg = message.serialize();

            msg.isStatusV3 = message.isStatusV3;
            msg.links = (message.getLinks()).map(link => ({
                link: link.href,
                isSuspicious: Boolean(link.suspiciousCharacters && link.suspiciousCharacters.size)
            }));

            if (msg.buttons) {
                msg.buttons = msg.buttons.serialize();
            }
            if (msg.dynamicReplyButtons) {
                msg.dynamicReplyButtons = JSON.parse(JSON.stringify(msg.dynamicReplyButtons));
            }
            if (msg.replyButtons) {
                msg.replyButtons = JSON.parse(JSON.stringify(msg.replyButtons));
            }

            if (typeof msg.id.remote === 'object') {
                msg.id = Object.assign({}, msg.id, { remote: msg.id.remote._serialized });
            }

            delete msg.pendingAckUpdate;

            return msg;
        };
         
        getChatModel = async chat => {
            let res = chat.serialize();
            res.isGroup = chat.isGroup;
            res.formattedTitle = chat.formattedTitle;
            res.isMuted = chat.mute && chat.mute.isMuted;

            if (chat.groupMetadata) {
                await window.Store.GroupMetadata.update(chat.id._serialized);
                res.groupMetadata = chat.groupMetadata.serialize();
            }

            delete res.msgs;
            delete res.msgUnsyncedButtonReplyMsgs;
            delete res.unsyncedButtonReplies;

            return res;
        };
         
        getChat = async chatId => {
            const chatWid = window.Store.WidFactory.createWid(chatId);
            const chat = await window.Store.Chat.find(chatWid);
            return await window.WWebJS.getChatModel(chat);

            /*
            const chat = window.Store.Chat.get(chatId); 
            return window.Util.getChatModelChange(chat);*/
        };

        getChats = async () => {
            const chats = window.Store.Chat.models;                      
            const chatPromises = chats.map(chat => window.WWebJS.getChatModel(chat));
            return await Promise.all(chatPromises);
           /* const chats = window.Store.Chat.models; 
            return chats.map(chat => window.Util.getChatModelChange(chat));*/
        };

        getContactModel = contact => {
            let res = contact.serialize();
            res.isBusiness = contact.isBusiness;

            if (contact.businessProfile) {
                res.businessProfile = contact.businessProfile.serialize();
            }

            res.isMe = contact.isMe;
            res.isUser = contact.isUser;
            res.isGroup = contact.isGroup;
            res.isWAContact = contact.isWAContact;
            res.isMyContact = contact.isMyContact;
            res.isBlocked = contact.isContactBlocked;
            res.userid = contact.userid;

            return res;
        };

        getContact = async contactId => {
            const wid = window.Store.WidFactory.createWid(contactId);
            const contact = await window.Store.Contact.find(wid);
            return window.WWebJS.getContactModel(contact);
        };

        getContacts = () => {
            const contacts = window.Store.Contact.models;
            return contacts.map(contact => window.WWebJS.getContactModel(contact));
        };

        mediaInfoToFile = ({data, mimetype, filename}) => {
            const binaryData = atob(data);

            const buffer = new ArrayBuffer(binaryData.length);
            const view = new Uint8Array(buffer);
            for (let i = 0; i < binaryData.length; i++) {
                view[i] = binaryData.charCodeAt(i);
            }

            const blob = new Blob([buffer], { type: mimetype });
            return new File([blob], filename, {
                type: mimetype,
                lastModified: Date.now()
            });
        };



        arrayBufferToBase64 = (arrayBuffer) => {
            let binary = '';
            const bytes = new Uint8Array(arrayBuffer);
            const len = bytes.byteLength;
            for (let i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            return window.btoa(binary);
        };

        getFileHash = async (data) => {
            let buffer = await data.arrayBuffer();
            const hashBuffer = await crypto.subtle.digest('SHA-256', buffer);
            return btoa(String.fromCharCode(...new Uint8Array(hashBuffer)));
        };

        generateHash = async (length) => {
            var result = '';
            var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
            var charactersLength = characters.length;
            for (var i = 0; i < length; i++) {
                result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
        };

         
        sendClearChat = async (chatId) => {
            let chat = window.Store.Chat.get(chatId);
            if (chat !== undefined) {
                await window.Store.SendClear.sendClear(chat, false);
                return true;
            }
            return false;
        };


        sendDeleteChat = async (chatId) => {
            let chat = window.Store.Chat.get(chatId);
            if (chat !== undefined) {
                await window.Store.SendDelete.sendDelete(chat);
                return true;
            }
            return false;
        };

        sendChatstate = async (state, chatId) => {
            switch (state) {
            case 'typing':
                await window.Store.Wap.sendChatstateComposing(chatId);
                break;
            case 'recording':
                await window.Store.Wap.sendChatstateRecording(chatId);
                break;
            case 'stop':
                await window.Store.Wap.sendChatstatePaused(chatId);
                break;
            default:
                throw 'Invalid chatstate';
            }

            return true;
        };

        getLabelModel = label => {
            let res = label.serialize();
            res.hexColor = label.hexColor;

            return res;
        };

        getLabels = () => {
            const labels = window.Store.Label.models;
            return labels.map(label => window.WWebJS.getLabelModel(label));
        };


        getLabel = (labelId) => {
            const label = window.Store.Label.get(labelId);
            return window.WWebJS.getLabelModel(label);
        };

         
        getChatLabels = async (chatId) => {
            const chat = await window.WWebJS.getChat(chatId);
            return (chat.labels || []).map(id => window.WWebJS.getLabel(id));
        };

        getOrderDetail = async (orderId, token) => {
            return window.Store.QueryOrder.queryOrder(orderId, 80, 80, token);
        };

        getProductMetadata = async (productId) => {
            let sellerId = window.Store.Conn.wid;
            let product = await window.Store.QueryProduct.queryProduct(sellerId, productId);
            if (product && product.data) {
                return product.data;
            }

            return undefined;
        };


        makeAllRead = () => {

            let Chats = window.Store.Chat.models;

            for (let chatIndex in Chats) {
                if (isNaN(chatIndex)) {
                    continue;
                }

                let chat = Chats[chatIndex];

                if (chat.unreadCount > 0) {
                    chat.markSeen();
                    window.Store.Wap.sendConversationSeen(chat.id, chat.getLastMsgKeyForAction(), chat.unreadCount - chat.pendingSeenCount);
                }
            }

            return undefined;

        }

    }

    window.WWebJS = new WWebJS();

     
}
