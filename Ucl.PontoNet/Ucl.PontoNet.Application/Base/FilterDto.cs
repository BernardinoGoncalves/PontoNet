using Ucl.PontoNet.Domain.Base.Interfaces;

namespace Ucl.PontoNet.Application.Base
{
    public class FilterDto : IFilterParameters
    {
        public int page { get; set; }

        public int per_page { get; set; }

        /// <summary>
        /// Campo de ordenação
        /// </summary>
        private string _sort;

        public string sort
        {
            get {
                if (_sort.IndexOf('-') == 0)
                {
                    return _sort.Remove(0, 1) + " desc";
                }
                else
                {
                    return _sort + " asc";
                }

            }
            set { _sort = value; }
        }

        public string filter { get; set; }

        public FilterDto()
        {
            this.sort = "Id";
            this.page = 1;
            this.per_page = int.MaxValue;
        }
    }
}