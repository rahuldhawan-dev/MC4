﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MapCall.SAP.TechnicalMasterWS
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails", ConfigurationName="TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN")]
    public interface TechnicalMaster_AccountDetails_Get_OB_SYN {
        // CODEGEN: Generating message contract since the operation TechnicalMaster_AccountDetails_Get_OB_SYN is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNResponse TechnicalMaster_AccountDetails_Get_OB_SYN(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://sap.com/xi/WebService/soap1.1", ReplyAction="*")]
        System.Threading.Tasks.Task<MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNResponse> TechnicalMaster_AccountDetails_Get_OB_SYNAsync(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails")]
    public partial class TechnicalMaster_AccountDetailsQuery : object, System.ComponentModel.INotifyPropertyChanged {
        private string premiseNumberField;
        
        private string equipmentField;
        
        private string installationTypeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string PremiseNumber {
            get {
                return this.premiseNumberField;
            }
            set {
                this.premiseNumberField = value;
                this.RaisePropertyChanged("PremiseNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Equipment {
            get {
                return this.equipmentField;
            }
            set {
                this.equipmentField = value;
                this.RaisePropertyChanged("Equipment");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string InstallationType {
            get {
                return this.installationTypeField;
            }
            set {
                this.installationTypeField = value;
                this.RaisePropertyChanged("InstallationType");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails")]
    public partial class TechnicalMaster_AccountDetailsInfo : object, System.ComponentModel.INotifyPropertyChanged {
        private TechnicalMaster_AccountDetailsInfoRecord[] recordField;
        
        private string exceptionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Record", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public TechnicalMaster_AccountDetailsInfoRecord[] Record {
            get {
                return this.recordField;
            }
            set {
                this.recordField = value;
                this.RaisePropertyChanged("Record");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Exception {
            get {
                return this.exceptionField;
            }
            set {
                this.exceptionField = value;
                this.RaisePropertyChanged("Exception");
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails")]
    public partial class TechnicalMaster_AccountDetailsInfoRecord : object, System.ComponentModel.INotifyPropertyChanged {
        private string installationTypeField;
        
        private string deviceField;
        
        private string meterSizeField;
        
        private string deviceLocationField;
        
        private string installationField;
        
        private string accountNoField;
        
        private string accountStatusAfterReviewField;
        
        private string customerField;
        
        private string ownerField;
        
        private string billingClassificationField;
        
        private string customerEmailField;
        
        private string phoneField;
        
        private string mobilePhoneField;
        
        private string serviceSizeField;
        
        private string equipmentField;
        
        private string manufacturerSerialNoField;
        
        private string criticalCareTypeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string InstallationType {
            get {
                return this.installationTypeField;
            }
            set {
                this.installationTypeField = value;
                this.RaisePropertyChanged("InstallationType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string Device {
            get {
                return this.deviceField;
            }
            set {
                this.deviceField = value;
                this.RaisePropertyChanged("Device");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string MeterSize {
            get {
                return this.meterSizeField;
            }
            set {
                this.meterSizeField = value;
                this.RaisePropertyChanged("MeterSize");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string DeviceLocation {
            get {
                return this.deviceLocationField;
            }
            set {
                this.deviceLocationField = value;
                this.RaisePropertyChanged("DeviceLocation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string Installation {
            get {
                return this.installationField;
            }
            set {
                this.installationField = value;
                this.RaisePropertyChanged("Installation");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string AccountNo {
            get {
                return this.accountNoField;
            }
            set {
                this.accountNoField = value;
                this.RaisePropertyChanged("AccountNo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string AccountStatusAfterReview {
            get {
                return this.accountStatusAfterReviewField;
            }
            set {
                this.accountStatusAfterReviewField = value;
                this.RaisePropertyChanged("AccountStatusAfterReview");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string Customer {
            get {
                return this.customerField;
            }
            set {
                this.customerField = value;
                this.RaisePropertyChanged("Customer");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string Owner {
            get {
                return this.ownerField;
            }
            set {
                this.ownerField = value;
                this.RaisePropertyChanged("Owner");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string BillingClassification {
            get {
                return this.billingClassificationField;
            }
            set {
                this.billingClassificationField = value;
                this.RaisePropertyChanged("BillingClassification");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=10)]
        public string CustomerEmail {
            get {
                return this.customerEmailField;
            }
            set {
                this.customerEmailField = value;
                this.RaisePropertyChanged("CustomerEmail");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=11)]
        public string Phone {
            get {
                return this.phoneField;
            }
            set {
                this.phoneField = value;
                this.RaisePropertyChanged("Phone");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=12)]
        public string MobilePhone {
            get {
                return this.mobilePhoneField;
            }
            set {
                this.mobilePhoneField = value;
                this.RaisePropertyChanged("MobilePhone");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=13)]
        public string ServiceSize {
            get {
                return this.serviceSizeField;
            }
            set {
                this.serviceSizeField = value;
                this.RaisePropertyChanged("ServiceSize");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=14)]
        public string Equipment {
            get {
                return this.equipmentField;
            }
            set {
                this.equipmentField = value;
                this.RaisePropertyChanged("Equipment");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=15)]
        public string ManufacturerSerialNo {
            get {
                return this.manufacturerSerialNoField;
            }
            set {
                this.manufacturerSerialNoField = value;
                this.RaisePropertyChanged("ManufacturerSerialNo");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=16)]
        public string CriticalCareType {
            get {
                return this.criticalCareTypeField;
            }
            set {
                this.criticalCareTypeField = value;
                this.RaisePropertyChanged("CriticalCareType");
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
    public partial class TechnicalMaster_AccountDetails_Get_OB_SYNRequest {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails", Order=0)]
        public MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsQuery TechnicalMaster_AccountDetails_Request;
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNRequest() {
        }
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNRequest(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsQuery TechnicalMaster_AccountDetails_Request) {
            this.TechnicalMaster_AccountDetails_Request = TechnicalMaster_AccountDetails_Request;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class TechnicalMaster_AccountDetails_Get_OB_SYNResponse {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://amwater.com/EAM/0015/MAPCALL/GetTechnicalMaster_AccountDetails", Order=0)]
        public MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsInfo TechnicalMaster_AccountDetails_Response;
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNResponse() {
        }
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNResponse(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsInfo TechnicalMaster_AccountDetails_Response) {
            this.TechnicalMaster_AccountDetails_Response = TechnicalMaster_AccountDetails_Response;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TechnicalMaster_AccountDetails_Get_OB_SYNChannel : MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TechnicalMaster_AccountDetails_Get_OB_SYNClient : System.ServiceModel.ClientBase<MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN>, MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN {
        public TechnicalMaster_AccountDetails_Get_OB_SYNClient() {
        }
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TechnicalMaster_AccountDetails_Get_OB_SYNClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNResponse MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN.TechnicalMaster_AccountDetails_Get_OB_SYN(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest request) {
            return base.Channel.TechnicalMaster_AccountDetails_Get_OB_SYN(request);
        }
        
        public MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsInfo TechnicalMaster_AccountDetails_Get_OB_SYN(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsQuery TechnicalMaster_AccountDetails_Request) {
            MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest inValue = new MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest();
            inValue.TechnicalMaster_AccountDetails_Request = TechnicalMaster_AccountDetails_Request;
            MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNResponse retVal = ((MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN)(this)).TechnicalMaster_AccountDetails_Get_OB_SYN(inValue);
            return retVal.TechnicalMaster_AccountDetails_Response;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNResponse> MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN.TechnicalMaster_AccountDetails_Get_OB_SYNAsync(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest request) {
            return base.Channel.TechnicalMaster_AccountDetails_Get_OB_SYNAsync(request);
        }
        
        public System.Threading.Tasks.Task<MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNResponse> TechnicalMaster_AccountDetails_Get_OB_SYNAsync(MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetailsQuery TechnicalMaster_AccountDetails_Request) {
            MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest inValue = new MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYNRequest();
            inValue.TechnicalMaster_AccountDetails_Request = TechnicalMaster_AccountDetails_Request;
            return ((MapCall.SAP.TechnicalMasterWS.TechnicalMaster_AccountDetails_Get_OB_SYN)(this)).TechnicalMaster_AccountDetails_Get_OB_SYNAsync(inValue);
        }
    }
}
