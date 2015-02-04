using System;
using System.Globalization;
using System.Web.Mvc;

namespace SunLine.Community.Web.ModelBinder
{
    /// <summary>
    /// Model binder konwertujący wartości liczbowe.
    /// </summary>
    public class DecimalModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Bindowanie modelu.
        /// </summary>
        /// <param name="controllerContext">Kontekst kontrolera.</param>
        /// <param name="bindingContext">Kontekst wiązania.</param>
        /// <returns>Wartość po konwersji.</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            string propertyName = bindingContext.ModelName;
            ValueProviderResult propertyValue = bindingContext.ValueProvider.GetValue(propertyName);

            if (propertyValue != null)
            {
                decimal convertedValue;
                if (decimal.TryParse(propertyValue.AttemptedValue, NumberStyles.Any, CultureInfo.CurrentCulture, out convertedValue))
                {
                    bindingContext.ModelState.Add(propertyName, new ModelState { Value = propertyValue });

                    return convertedValue;
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}