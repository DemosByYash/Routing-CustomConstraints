using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Linq;
using Microsoft.AspNetCore.Routing.Matching;


namespace RoutingProject.CustomConstraints
{
    public class IdConstraint: IRouteConstraint
    {
        private static readonly HashSet<int> _validIds = new();

        public static void SetConstraint(IEnumerable<int> CountryId)
        {
            foreach (int id in CountryId)
            {
                _validIds.Add(id);
            }
        }


        public bool Match(HttpContext httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if(_validIds.Contains(Convert.ToInt32(values[routeKey])))
            {
                return true;
            }
            return false;
        }
    }
}
