/// <reference path="~/Scripts/ESG/_references.js" />

ESG.Common = ESG.Common || {};

ESG.Common.bindViewModel = function (selector, viewModel) {

    var $element = $(selector);
    if ($element && $element[0]) {
        if (viewModel) {
            ko.cleanNode($element[0]);
            ko.applyBindings(viewModel, $element[0]);
        }
    }

};