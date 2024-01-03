﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapCall.SAP.WorkOrderStatusUpdateWS
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates", ConfigurationName="WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC")]
    public interface WO_StatusUpdate_OB_SYC {
        // CODEGEN: Generating message contract since the operation WO_StatusUpdate_OB_SYC is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCResponse WO_StatusUpdate_OB_SYC(MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        System.Threading.Tasks.Task<MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCResponse> WO_StatusUpdate_OB_SYCAsync(MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates")]
    public partial class WOStatusUpdateQuery : object, System.ComponentModel.INotifyPropertyChanged {
        private string sourceIdentifierField;
        
        private WOStatusUpdateQueryRecord[] recordField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SourceIdentifier {
            get {
                return this.sourceIdentifierField;
            }
            set {
                this.sourceIdentifierField = value;
                this.RaisePropertyChanged("SourceIdentifier");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Record", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public WOStatusUpdateQueryRecord[] Record {
            get {
                return this.recordField;
            }
            set {
                this.recordField = value;
                this.RaisePropertyChanged("Record");
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates")]
    public partial class WOStatusUpdateQueryRecord : object, System.ComponentModel.INotifyPropertyChanged {
        private string workOrderNoField;
        
        private string operationNoField;
        
        private string assignmentStartField;
        
        private string assignmentFinishField;
        
        private string status_NumberField;
        
        private string status_NonNumberField;
        
        private string assignedEngineerField;
        
        private string dispatcherIdField;
        
        private string engineerIdField;
        
        private string itemTimeStampField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string WorkOrderNo {
            get {
                return this.workOrderNoField;
            }
            set {
                this.workOrderNoField = value;
                this.RaisePropertyChanged("WorkOrderNo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string OperationNo {
            get {
                return this.operationNoField;
            }
            set {
                this.operationNoField = value;
                this.RaisePropertyChanged("OperationNo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string AssignmentStart {
            get {
                return this.assignmentStartField;
            }
            set {
                this.assignmentStartField = value;
                this.RaisePropertyChanged("AssignmentStart");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string AssignmentFinish {
            get {
                return this.assignmentFinishField;
            }
            set {
                this.assignmentFinishField = value;
                this.RaisePropertyChanged("AssignmentFinish");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Status_Number {
            get {
                return this.status_NumberField;
            }
            set {
                this.status_NumberField = value;
                this.RaisePropertyChanged("Status_Number");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string Status_NonNumber {
            get {
                return this.status_NonNumberField;
            }
            set {
                this.status_NonNumberField = value;
                this.RaisePropertyChanged("Status_NonNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string AssignedEngineer {
            get {
                return this.assignedEngineerField;
            }
            set {
                this.assignedEngineerField = value;
                this.RaisePropertyChanged("AssignedEngineer");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string DispatcherId {
            get {
                return this.dispatcherIdField;
            }
            set {
                this.dispatcherIdField = value;
                this.RaisePropertyChanged("DispatcherId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string EngineerId {
            get {
                return this.engineerIdField;
            }
            set {
                this.engineerIdField = value;
                this.RaisePropertyChanged("EngineerId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string ItemTimeStamp {
            get {
                return this.itemTimeStampField;
            }
            set {
                this.itemTimeStampField = value;
                this.RaisePropertyChanged("ItemTimeStamp");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates")]
    public partial class WOStatusUpdateStatus : object, System.ComponentModel.INotifyPropertyChanged {
        private string sAPStatusCodeField;
        
        private string sAPStatusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string SAPStatusCode {
            get {
                return this.sAPStatusCodeField;
            }
            set {
                this.sAPStatusCodeField = value;
                this.RaisePropertyChanged("SAPStatusCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string SAPStatus {
            get {
                return this.sAPStatusField;
            }
            set {
                this.sAPStatusField = value;
                this.RaisePropertyChanged("SAPStatus");
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
    public partial class WO_StatusUpdate_OB_SYCRequest {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates", Order=0)]
        public MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateQuery WOStatusUpdate_Request;
        
        public WO_StatusUpdate_OB_SYCRequest() {
        }
        
        public WO_StatusUpdate_OB_SYCRequest(MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateQuery WOStatusUpdate_Request) {
            this.WOStatusUpdate_Request = WOStatusUpdate_Request;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class WO_StatusUpdate_OB_SYCResponse {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://amwater.com/PTB/200011/MAPCALL/WOStatusUpdates", Order=0)]
        public MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateStatus WOStatusUpdate_Response;
        
        public WO_StatusUpdate_OB_SYCResponse() {
        }
        
        public WO_StatusUpdate_OB_SYCResponse(MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateStatus WOStatusUpdate_Response) {
            this.WOStatusUpdate_Response = WOStatusUpdate_Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WO_StatusUpdate_OB_SYCChannel : MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WO_StatusUpdate_OB_SYCClient : System.ServiceModel.ClientBase<MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC>, MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC {
        public WO_StatusUpdate_OB_SYCClient() {
        }
        
        public WO_StatusUpdate_OB_SYCClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WO_StatusUpdate_OB_SYCClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WO_StatusUpdate_OB_SYCClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WO_StatusUpdate_OB_SYCClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCResponse MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC.WO_StatusUpdate_OB_SYC(MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest request) {
            return base.Channel.WO_StatusUpdate_OB_SYC(request);
        }
        
        public MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateStatus WO_StatusUpdate_OB_SYC(MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateQuery WOStatusUpdate_Request) {
            MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest inValue = new MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest();
            inValue.WOStatusUpdate_Request = WOStatusUpdate_Request;
            MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCResponse retVal = ((MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC)(this)).WO_StatusUpdate_OB_SYC(inValue);
            return retVal.WOStatusUpdate_Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCResponse> MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC.WO_StatusUpdate_OB_SYCAsync(MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest request) {
            return base.Channel.WO_StatusUpdate_OB_SYCAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCResponse> WO_StatusUpdate_OB_SYCAsync(MapCall.SAP.WorkOrderStatusUpdateWS.WOStatusUpdateQuery WOStatusUpdate_Request) {
            MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest inValue = new MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYCRequest();
            inValue.WOStatusUpdate_Request = WOStatusUpdate_Request;
            return ((MapCall.SAP.WorkOrderStatusUpdateWS.WO_StatusUpdate_OB_SYC)(this)).WO_StatusUpdate_OB_SYCAsync(inValue);
        }
    }
}