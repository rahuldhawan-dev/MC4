﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapCall.SAP.NotificationStatusWS
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://amwater.com/EAM/0012/MAPCALL/CancelUpdateNotification", ConfigurationName="NotificationStatusWS.NotificationStatus_OB_SYN")]
    public interface NotificationStatus_OB_SYN {
        // CODEGEN: Generating message contract since the operation NotificationStatus_OB_SYN is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNResponse NotificationStatus_OB_SYN(MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        System.Threading.Tasks.Task<MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNResponse> NotificationStatus_OB_SYNAsync(MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://amwater.com/EAM/0012/MAPCALL/CancelUpdateNotification")]
    public partial class CancelUpdate_NotificationUpdateRequest : object, System.ComponentModel.INotifyPropertyChanged {
        private string sAPNotificationNoField;
        
        private string completeField;
        
        private string cancelField;
        
        private string remarksField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SAPNotificationNo {
            get {
                return this.sAPNotificationNoField;
            }
            set {
                this.sAPNotificationNoField = value;
                this.RaisePropertyChanged("SAPNotificationNo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Complete {
            get {
                return this.completeField;
            }
            set {
                this.completeField = value;
                this.RaisePropertyChanged("Complete");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string Cancel {
            get {
                return this.cancelField;
            }
            set {
                this.cancelField = value;
                this.RaisePropertyChanged("Cancel");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string Remarks {
            get {
                return this.remarksField;
            }
            set {
                this.remarksField = value;
                this.RaisePropertyChanged("Remarks");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://amwater.com/EAM/0012/MAPCALL/CancelUpdateNotification")]
    public partial class NotificationStatusStatus : object, System.ComponentModel.INotifyPropertyChanged {
        private string notificationIDField;
        
        private string sAPMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string NotificationID {
            get {
                return this.notificationIDField;
            }
            set {
                this.notificationIDField = value;
                this.RaisePropertyChanged("NotificationID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string SAPMessage {
            get {
                return this.sAPMessageField;
            }
            set {
                this.sAPMessageField = value;
                this.RaisePropertyChanged("SAPMessage");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class NotificationStatus_OB_SYNRequest {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://amwater.com/EAM/0012/MAPCALL/CancelUpdateNotification", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("UpdateRequest", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public MapCall.SAP.NotificationStatusWS.CancelUpdate_NotificationUpdateRequest[] NotificationStatus_Request;
        
        public NotificationStatus_OB_SYNRequest() {
        }
        
        public NotificationStatus_OB_SYNRequest(MapCall.SAP.NotificationStatusWS.CancelUpdate_NotificationUpdateRequest[] NotificationStatus_Request) {
            this.NotificationStatus_Request = NotificationStatus_Request;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class NotificationStatus_OB_SYNResponse {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://amwater.com/EAM/0012/MAPCALL/CancelUpdateNotification", Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Status", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public MapCall.SAP.NotificationStatusWS.NotificationStatusStatus[] NotificationStatus_Response;
        
        public NotificationStatus_OB_SYNResponse() {
        }
        
        public NotificationStatus_OB_SYNResponse(MapCall.SAP.NotificationStatusWS.NotificationStatusStatus[] NotificationStatus_Response) {
            this.NotificationStatus_Response = NotificationStatus_Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NotificationStatus_OB_SYNChannel : MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NotificationStatus_OB_SYNClient : System.ServiceModel.ClientBase<MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN>, MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN {
        public NotificationStatus_OB_SYNClient() {
        }
        
        public NotificationStatus_OB_SYNClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NotificationStatus_OB_SYNClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NotificationStatus_OB_SYNClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NotificationStatus_OB_SYNClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNResponse MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN.NotificationStatus_OB_SYN(MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest request) {
            return base.Channel.NotificationStatus_OB_SYN(request);
        }
        
        public MapCall.SAP.NotificationStatusWS.NotificationStatusStatus[] NotificationStatus_OB_SYN(MapCall.SAP.NotificationStatusWS.CancelUpdate_NotificationUpdateRequest[] NotificationStatus_Request) {
            MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest inValue = new MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest();
            inValue.NotificationStatus_Request = NotificationStatus_Request;
            MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNResponse retVal = ((MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN)(this)).NotificationStatus_OB_SYN(inValue);
            return retVal.NotificationStatus_Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNResponse> MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN.NotificationStatus_OB_SYNAsync(MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest request) {
            return base.Channel.NotificationStatus_OB_SYNAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNResponse> NotificationStatus_OB_SYNAsync(MapCall.SAP.NotificationStatusWS.CancelUpdate_NotificationUpdateRequest[] NotificationStatus_Request) {
            MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest inValue = new MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYNRequest();
            inValue.NotificationStatus_Request = NotificationStatus_Request;
            return ((MapCall.SAP.NotificationStatusWS.NotificationStatus_OB_SYN)(this)).NotificationStatus_OB_SYNAsync(inValue);
        }
    }
}
