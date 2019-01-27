
(function () {
    'use strict';
    //onloadCallback();
    function Ctrl($scope) {
        $scope.age = 24;
    }

    $("#dvFpage").hide();

    var ctrlDown = false;
    var ctrlKey = 17, vKey = 86, cKey = 67;

    $(document).keydown(function (e) {
        //alert(e.keyCode);
        if (e.keyCode == ctrlKey) ctrlDown = true;
    }).keyup(function (e) {
        if (e.keyCode == ctrlKey) ctrlDown = false;
    });

    $("#page-top").keydown(function (e) {
        if (ctrlDown && (e.keyCode == vKey || e.keyCode == cKey)) return false;
    });

    var app5 = angular.module('instalogix_ds', ['ngDialog'
        , 'formly'
        , 'formlyBootstrap'
        , 'ngMessages'
        , 'ngRoute'

       // , 'ui.bootstrap'
       // , 'ngSanitize'

        //, 'ui.select'

    ]
    );


    app5.factory('jsonService', function jsonService($http) {
        return {
            getJSON: getJSON
        };

        function getJSON(url) {
            return $http.get('/odata/' + url);
        }
    });

   // var app = angular.module('instalogix_ds', []);
        app5.controller('MainCtrlaa', ['$scope', Ctrl]);


        function resetImage(vm) {
            loadImage(vm.options.formState.imageTextId, vm.options.formState.imageUrl);
        }
        function loadImage(id, url) {
            $("#" + id).attr("src", url + $.now());
        }
        function resetError(vm, showError) {
            console.log(JSON.stringify(vm.json));
            if (vm.json && vm.json.success) {
                console.log('ZZ::' + vm.json.success + '/' + vm.errorId + '/' + vm.json.reference);
                $("#" + vm.errorId).addClass("successMessage");
                $("#" + vm.errorId).removeClass("errorMessage");
                var ref = '';
                if (vm.json.reference) {
                    ref = 'Reference No:' + vm.json.reference;
                }
                $("#" + vm.errorId).text("Request submitted successfuly. " + ref);
                if (vm.json.redirect) {
                    window.location = vm.json.redirect;
                }
                vm.options.resetModel();
            }
            else if (showError && vm.json.errors) {
                var items = $.map(vm.json.errors, function (error) {
                    console.log('Error Detail:- ' + error);
                    return error;
                }).join(',');
                $("#" + vm.errorId).text(items);
                $("#" + vm.errorId).addClass("errorMessage");

            }
            else {
                $("#" + vm.errorId).removeClass("errorMessage");
                $("#" + vm.errorId).text(vm.model.message);
                vm.options.resetModel();
            }
            resetImage(vm);
        }

        function onSubmit() {
            var vm = this;
            $("#" + vm.errorId).removeClass("errorMessage");
            $("#" + vm.errorId).removeClass("successMessage");
            var token = $('[name=__RequestVerificationToken]').val();
            // alert(token);
            this.jx = $.ajax(
                {
                    dataType: "json",
                    cache: false,
                    contentType: "application/json",
                    type: 'POST',
                    url: vm.url,
                    headers: { "__RequestVerificationToken": token },
                    data: JSON.stringify(vm.model)
                }).done(
                function (json) {
                    console.log(JSON.stringify(json));
                    vm.json = json;
                    vm.resetError(vm, true);
                }).error(function () {
                    vm.json = { errors: { error: 'Your request could not be submitted due to server error.' } };
                    vm.resetError(vm, true);
                });
        };

        //var ajaxlogin = new ajaxLogin();
        //$("#loginForm").submit(ajaxlogin.formSubmitHandler);
        //var ak = '2222333';
        app5.controller('MainCtrl', function ($scope, ngDialog) {
            $scope.open = (function (val) {
                // $("#loginForm").submit(formSubmitHandler);
                // alert(ajaxlogin.msg);
                var objModal = ngDialog.open({
                    template: 'firstDialog' + val,
                    controller: 'FirstDialogCtrl',
                    className: 'ngdialog-theme-default ngdialog-theme-custom ' + (val == 'R' ? 'ngdialog-theme-defaultR' : ''),//ngdialog-theme-default ngdialog-theme-custom'
                });//.then($("#loginForm").submit(alert(2222)));
            });
        });


        app5.controller('FirstDialogCtrl', function ($scope, ngDialog) {
            $scope.next = function (val) {
                // $("#registerForm").submit(formSubmitHandler);
                //alert(ak);
                ngDialog.close('ngdialog1');
                ngDialog.open({
                    template: 'secondDialog' + val,
                    controller: 'SecondDialogCtrl',
                    className: 'ngdialog-theme-flat ngdialog-theme-custom'
                });
            };

        });


        app5.controller('SecondDialogCtrl', function ($scope, ngDialog) {
            $scope.prev = function (val) {
                ngDialog.close('ngdialog1');
                ngDialog.open({
                    template: 'firstDialog' + val,
                    controller: 'FirstDialogCtrl',
                    className: 'ngdialog-theme-default ngdialog-theme-custom ' + (val == 'R' ? 'ngdialog-theme-defaultR' : ''),//ngdialog-theme-default ngdialog-theme-custom'
                });
            };
        });

        app5.directive('ngLike', function () {
            return {
                restrict: 'E',
                link: function (scope, elem, attrs) {
                    elem.on('click', function () {
                        window.open(attr.href, 'Share', 'width=600,height=400,resizable=yes');
                    });
                }
            };
        });

        app5.run(function (formlyConfig) {

            formlyConfig.setWrapper({
                name: 'inputWrapper',
                template: '<div ng-class="to.changeColor==\'red\'? \'redBorder\' : \'otherBorder\'"><formly-transclude></formly-transclude>{{to.label}}</div>'
            });


            formlyConfig.setType(
                { name: "imageText", templateUrl: "imageText.html" });
            formlyConfig.setType({ name: "messageAreaLabel", templateUrl: "messageAreaLabel.html" });


            // set templates here
            formlyConfig.setType({
                name: 'matchField',
                apiCheck: function () {
                    return {
                        data: {
                            fieldToMatch: formlyExampleApiCheck.string
                        }
                    }
                },
                apiCheckOptions: {
                    prefix: 'matchField type'
                },
                defaultOptions: function matchFieldDefaultOptions(options) {
                    return {
                        extras: {
                            validateOnModelChange: true
                        },
                        expressionProperties: {
                            'templateOptions.disabled': function (viewValue, modelValue, scope) {
                                var matchField = find(scope.fields, 'key', options.data.fieldToMatch);
                                if (!matchField) {
                                    throw new Error('Could not find a field for the key ' + options.data.fieldToMatch);
                                }
                                var model = options.data.modelToMatch || scope.model;
                                var originalValue = model[options.data.fieldToMatch];
                                var invalidOriginal = matchField.formControl && matchField.formControl.$invalid;
                                return !originalValue || invalidOriginal;
                            }
                        },
                        validators: {
                            fieldMatch: {
                                expression: function (viewValue, modelValue, fieldScope) {
                                    var value = modelValue || viewValue;
                                    var model = options.data.modelToMatch || fieldScope.model;
                                    return value === model[options.data.fieldToMatch];
                                },
                                message: options.data.matchFieldMessage || '"Must match"'
                            }
                        }
                    };

                    function find(array, prop, value) {
                        var foundItem;
                        array.some(function (item) {
                            if (item[prop] === value) {
                                foundItem = item;
                            }
                            return !!foundItem;
                        });
                        return foundItem;
                    }
                }
            });


            formlyConfig.setType({
                name: 'input1',
                template: '<input ng-model="model[options.key]"></input>',
                wrapper: ['ngMessages']
            });

            formlyConfig.setWrapper({
                name: 'ngMessages',
                template: '<formly-transclude></formly-transclude><div ng-messages="fc.$error" ><div ng-repeat="(name, message) in ::options.validation.messages" ng-message="{{::name}}"  style="margin-top:-2px ;border:1px solid red; width:100%; height:1px"></div></div>'
                //template: '<formly-transclude></formly-transclude><div ng-messages="fc.$error">'+
                //    '<div ng-repeat="(name, message) in ::options.validation.messages" ng-message="{{::name}}">{{message(fc.$viewValue, fc.$modelValue, this)}}</div></div>'
            });

            formlyConfig.setType({
                name: 'ui-select',
                extends: 'select',
                templateUrl: 'async-ui-select-type.html'
            });
        });

        // function definition
        function ontodekSubmit() {
            var vm = this;
            this.jx = $.ajax(
                {
                    dataType: "json",
                    cache: false,
                    contentType: "application/json",
                    type: 'POST',
                    url: vm.url,
                    data: JSON.stringify(vm.model)
                }).done(
                function (json) {
                    vm.json = json;
                    vm.resetError(vm, true);
                }).error(function () {
                    vm.json = { errors: { error: 'Your request could not be submitted due to server error.' } };
                    vm.resetError(vm, true);
                });

            //// alert(vm.url);
            //// vm.options.formState.className = "successMessage";
            ////   vm.options.formState.message = 'Request Form submitting...';
            //this.jx = $.ajax({
            //    dataType: "json",
            //    cache: false,
            //    contentType: "application/json",
            //    type: 'POST',
            //    url: vm.url,// "/Account/JsonRequestItem",///odata/RequestItems",
            //    data: JSON.stringify(vm.model)//$form.serializeArray())
            //}).done((function (json) {
            //    json = json || {};
            //    vm.options.formState.message = '111Error: Your request could not be submitted due to server error';

            //    json.vm = vm;
            //    json.KeyFieldName = "RequestItemId";
            //    postCallBack(json);

            //})).error(function () {
            //    vm.options.formState.className = "errorMessage";
            //    vm.options.formState.message = 'Error: Your request could not be submitted due to server error';
            //});
            //  jx.vm = vm;
        }


        function postCallBack(json) {

            json.vm.options.formState.message = "Request Submitted";
            if (json[json.KeyFieldName] !== undefined) {
                json.vm.options.formState.className = "successMessage";
                json.vm.options.formState.message = 'Your request submitted successfully (Reference No:' + json[json.KeyFieldName] + ')';
            }
            else {
                if (json.success) {
                    json.vm.options.resetModel();
                    window.location = json.redirect || location.href;
                }
                else if (json.errors) {
                    var items = $.map(json.errors, function (error) {
                        console.log('__--' + error);
                        return error;
                    }).join(',');
                    alert(items);
                    json.vm.options.formState.className = "errorMessage";
                    json.vm.options.formState.message = 'Error: ' + items;
                }
                else {
                    json.vm.options.formState.className = "errorMessage";
                    json.vm.options.formState.message = 'Error: Your request could not be submitted';
                }
            }
        }

        app5.controller('contactController', function MainCtrl(formlyVersion) {
            var vm = this;
            // funcation assignment
            vm.url = "/Account/JsonRequestItem";
            vm.model = {
                message: "Request Form"
            };
            vm.resetError = resetError;
            vm.onSubmit = onSubmit;
            vm.resetImage = resetImage;
            vm.errorId = 'errorRequestmsgID';//New Name
            vm.options = {
                formState: {
                    message: vm.model.message,
                    awesomeIsForced: false,
                    className: "generalMessage",
                    refreshImage: loadImage,
                    imageTextId: 'contactUsImage',
                    imageUrl: '/comcomps/CaptchaHandler.ashx?type=contact&ta='
                }
            };

            vm.fields = [
                {
                    key: '',
                    type: 'messageAreaLabel',
                    wrapper: ['ngMessages'],
                    templateOptions: {
                        id: vm.errorId,
                        label: vm.model.message,
                    }
                    ,
                    expressionProperties: {
                        'templateOptions.label': function (viewValue, modelValue, scope) {
                            return vm.model.message;
                        },
                        'templateOptions.className': function (viewValue, modelValue, scope) {
                            return scope.formState.className;
                        }
                    },
                },
                {
                    key: 'RequestorName',
                    type: 'input',

                    templateOptions: {
                        label: 'Your Name',
                        placeholder: 'Enter your name',
                        focus: true,
                    }
                },
                {
                    key: 'Email',
                    type: 'input',
                    templateOptions: {
                        type: 'email',
                        label: 'Email',
                        placeholder: 'Enter your email address',
                        required: true,
                    }
                },
                {
                    key: 'PhoneNumber',
                    type: 'input',
                    templateOptions: {
                        label: 'Phone',
                        placeholder: 'Enter your phone number'
                    }
                },
                {
                    key: 'Country',
                    type: 'input',
                    templateOptions: {
                        label: 'Country',
                        placeholder: 'Enter country name'
                    }
                }
                ,
                {
                    key: 'RequestTypeId',
                    type: "ui-select",
                    templateOptions: {
                        label: 'Request',
                        options: [],

                        valueProp: 'RequestTypeId',
                        labelProp: 'Title',


                        placeholder: 'Select Request Type'
                    },
                    controller: function ($scope, jsonService) {
                        $scope.to.loading = jsonService.getJSON('RequestTypes?$select=RequestTypeId,Title').then(function (response) {
                            $scope.to.options = response.data.value;
                            console.log(JSON.stringify(response));
                            return response;
                        });
                    }
                },
                {
                    key: 'RequestDetail',
                    type: "textarea",
                    templateOptions: {
                        label: 'Detail',
                        placeholder: 'Please enter detail',
                        lines: 6,
                        required: true,
                    }
                },
                {
                    key: 'RequestPriorityId',
                    type: "ui-select",
                    templateOptions: {
                        label: 'Priority',
                        options: [],

                        valueProp: 'RequestPriorityId',
                        labelProp: 'Title',


                        placeholder: 'Select Request Priority'
                    },
                    controller: function ($scope, jsonService) {
                        $scope.to.loading = jsonService.getJSON('RequestPriorities?$select=RequestPriorityId,Title').then(function (response) {
                            $scope.to.options = response.data.value;
                            console.log(JSON.stringify(response));
                            return response;
                        });
                    }
                },
                //{
                //    key: 'RequestPriorityId',
                //    type: 'select',
                //    templateOptions: {
                //        label: 'Priority',
                //        options: [],
                //        valueProp: 'RequestPriorityId',
                //        labelProp: 'Title',
                //        required: true,
                //        placeholder: 'Select Priority'
                //    },
                //    controller: /* @ngInject */ function ($scope, jsonService) {
                //        $scope.to.loading = jsonService.getJSON('RequestPriorities').then(function (response) {
                //            $scope.to.options = response.data.value;
                //            // note, the line above is shorthand for:
                //            // $scope.options.templateOptions.options = data;
                //            return response;
                //        });
                //    }
                //},
                {
                    key: 'ImageText',
                    type: 'imageText',
                    templateOptions: {
                        label: 'Image Text',
                        placeholder: 'Enter Image Text',
                        required: true,
                        imageUrl: vm.options.formState.imageUrl,//"/comcomps/CaptchaHandler.ashx?type=&t=",

                        id: vm.options.formState.imageTextId //"loginImage"
                    },
                    expressionProperties: {
                        'templateOptions.refreshImage': function (viewValue, modelValue, scope) {
                            return scope.formState.refreshImage;
                        },
                    },
                },
            ];



        });


        app5.controller('registerController', function MainCtrl(formlyVersion) {
            var vm = this;
            vm.url = "/Account/JsonRegister";
            vm.model = {
                message: "Signup Form"
            };
            vm.resetError = resetError;
            vm.onSubmit = onSubmit;
            vm.resetImage = resetImage;
            vm.errorId = 'errorrRegistermsgID';
            vm.options = {
                formState: {
                    message: vm.model.message,
                    awesomeIsForced: false,
                    className: "generalMessage",
                    refreshImage: loadImage,
                    imageTextId: 'registerImage',
                    imageUrl: '/comcomps/CaptchaHandler.ashx?type=register&ta='
                }
            };

            vm.fields = [
                {
                    key: '',
                    type: 'messageAreaLabel',
                    wrapper: ['ngMessages'],
                    templateOptions: {
                        id: vm.errorId,
                        label: vm.model.message,
                    }
                    ,
                    expressionProperties: {
                        'templateOptions.label': function (viewValue, modelValue, scope) {
                            return vm.model.message;
                        },
                        'templateOptions.className': function (viewValue, modelValue, scope) {
                            return scope.formState.className;
                        }
                    },
                },
                {
                    key: 'DisplayName',
                    type: 'input',


                    templateOptions: {
                        label: 'Your Name',
                        placeholder: 'Enter your name',
                        required: true,
                        focus: true,
                    }
                },
                {
                    key: 'UserName',
                    type: 'input',
                    templateOptions: {
                        label: 'Email',
                        type: 'email',
                        placeholder: 'Enter your email address for login',
                        required: true,
                    }
                },
                {
                    key: 'password',
                    type: 'input',
                    templateOptions: {
                        type: 'password',
                        label: 'Password',
                        placeholder: 'Must be at least 3 characters',
                        required: true,
                        minlength: 3
                    }
                },
                {
                    key: 'confirmPassword',
                    type: 'input',
                    optionsTypes: ['matchField'],
                    model: vm.confirmationModel,
                    templateOptions: {
                        type: 'password',
                        label: 'Confirm Password',
                        placeholder: 'Please re-enter your password',
                        required: true
                    },
                    data: {
                        fieldToMatch: 'password',
                        modelToMatch: vm.model
                    }
                },


                {
                    key: 'ImageText',
                    type: 'imageText',
                    templateOptions: {
                        label: 'Image Text',
                        placeholder: 'Enter Image Text',
                        required: true,
                        imageUrl: vm.options.formState.imageUrl,//"/comcomps/CaptchaHandler.ashx?type=&t=",

                        id: vm.options.formState.imageTextId //"loginImage"
                    },
                    expressionProperties: {
                        'templateOptions.refreshImage': function (viewValue, modelValue, scope) {
                            return scope.formState.refreshImage;
                        },
                    },
                }

            ];



        });

        app5.controller('loginController', function MainCtrl(formlyVersion, $scope, formlyValidationMessages) {
            var vm = this;
            vm.model = {
                message: "Login Form"
            };
            vm.url = "/Account/JsonLogin";
            vm.resetError = resetError;
            vm.onSubmit = onSubmit;
            vm.resetImage = resetImage;
            vm.errorId = 'errormsgID';
            vm.options = {
                formState: {
                    message: vm.model.message,
                    awesomeIsForced: false,
                    className: "generalMessage",
                    refreshImage: loadImage,
                    imageTextId: 'loginImage',
                    imageUrl: '/comcomps/CaptchaHandler.ashx?type=&ta='
                }
            };
            vm.fields = [
                {
                    key: '',
                    type: 'messageAreaLabel',
                    wrapper: ['ngMessages'],
                    templateOptions: {
                        id: vm.errorId,
                        label: vm.model.message,
                    }
                    ,
                    expressionProperties: {
                        'templateOptions.label': function (viewValue, modelValue, scope) {
                            return vm.model.message;
                        },
                        'templateOptions.className': function (viewValue, modelValue, scope) {
                            return scope.formState.className;
                        }
                    },
                },
                {
                    key: 'UserName',
                    type: 'input',

                    templateOptions: {
                        label: 'Login Name/Email',
                        placeholder: 'Enter your login name or email',
                        required: true,
                        focus: true,
                    }
                },
                {
                    key: 'Password',
                    type: 'input',
                    templateOptions: {
                        type: 'password',
                        label: 'Password',
                        placeholder: 'Enter your Password',
                        required: true,
                    }
                },
                {
                    key: 'ImageText',
                    type: 'imageText',
                    templateOptions: {
                        label: 'Image Text',
                        placeholder: 'Enter Image Text',
                        required: true,
                        imageUrl: vm.options.formState.imageUrl,//"/comcomps/CaptchaHandler.ashx?type=&t=",
                        id: vm.options.formState.imageTextId //"loginImage"
                    },
                    expressionProperties: {
                        'templateOptions.refreshImage': function (viewValue, modelValue, scope) {
                            return scope.formState.refreshImage;
                        },
                    },
                }
            ];
        });


})();
