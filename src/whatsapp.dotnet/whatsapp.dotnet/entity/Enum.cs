namespace whatsapp.dotnet
{
    public enum InstanceType
    {
        notstarted,
        waiting,
        processing,
        pausando,
        expired
    }

    public enum EventsMessage
    {
        AUTHENTICATED,
        AUTHENTICATION_FAILURE,
        READY,
        MESSAGE_RECEIVED,
        MESSAGE_CREATE,
        MESSAGE_REVOKED_EVERYONE,
        MESSAGE_REVOKED_ME,
        MESSAGE_ACK,
        MEDIA_UPLOADED,
        GROUP_JOIN,
        GROUP_LEAVE,
        GROUP_UPDATE,
        QR_RECEIVED,
        DISCONNECTED,
        STATE_CHANGED,
        BATTERY_CHANGED
    }
    public enum ProcessingType
    {
        qrcodeProcessing, /*processing whatssap*/
        whatsappValidationOk, /*init started monitory*/
        initiatedwhatsapmonitory, /*init started monitory*/
        whatsappMonitory, /*init started monitory*/
        bateryChange, /*init started monitory*/
        whatsappExit, /*init started monitory*/
        whatsappErroRestart /*init started monitory*/
    }
    
    public enum SubNotificationType
    {
        msgGroup,
        msgPrivate,
        msgImageType
    }


    public enum ContentTypeWS
    {
        stringResult
    }

    public enum StatusTransaction
    {
        Ok,
        Erro
    }
    
    public enum StatusAgentBot {
        ok,
        expired,
        block
    }
}