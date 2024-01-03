<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientIDRepository.ascx.cs" Inherits="LINQTo271.Common.ClientIDRepository" %>
<script type="text/javascript">
    var CLIENT_IDS = [<%= ClientIDList %>];

    var ClientIDRepository = {
        lookup: function(id) {
            for (var i = CLIENT_IDS.length - 1; i >= 0; --i)
                if (CLIENT_IDS[i].serverID == id)
                    return CLIENT_IDS[i].clientID;
            return null;
        }
    };

    function getServerElement(id) {
        return $('#' + ClientIDRepository.lookup(id));
    }
</script>
