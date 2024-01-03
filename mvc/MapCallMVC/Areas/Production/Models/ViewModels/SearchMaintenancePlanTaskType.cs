using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchMaintenancePlanTaskType : SearchSet<MaintenancePlanTaskType>
    {
        #region Properties

        public SearchString Description { get; set; }

        public SearchString Abbreviation { get; set; }

        public SearchString Code { get; set; }

        public bool? IsActive { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Accepts a Code string (Ex "P005", "5", "005", etc) and returns an integer Id value
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static int GetIdFromCode(string code)
        {
            var id = new string(
                code.Where(char.IsDigit)
                    .ToArray());

            return string.IsNullOrEmpty(id)
                ? 0
                : Convert.ToInt32(id);
        }

        #endregion

        #region Exposed Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);

            if (Code?.Value != null)
            {
                mapper.MappedProperties["Code"].ActualName = "Id";
                mapper.MappedProperties["Code"].Value = GetIdFromCode(Code.Value);
            }
        }

        #endregion
    }
}