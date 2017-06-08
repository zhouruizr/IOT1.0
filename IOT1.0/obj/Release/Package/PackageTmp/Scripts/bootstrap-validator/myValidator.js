
var formValidator = {
    init: function (formId) {
        $(formId).bootstrapValidator({
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                username: {
                    validators: {
                        notEmpty: {
                            message: '用户名必须填写。'
                        },
                        stringLength: {
                            min: 6,
                            max: 20,
                            message: '用户名长度最少6个字符，最多20个字符。'
                        },
                        regexp: {
                            regexp: /^(\w)*$/,
                            message: '用户名只能是字母和数字,不能包含空格和其他字符。'
                        }
                    }
                },
                password: {
                    validators: {
                        notEmpty: {
                            message: '密码必须填写。'
                        },
                        stringLength: {
                            min: 6,
                            max: 20,
                            message: '密码长度最少6个字符，最多20个字符。'
                        },
                        regexp: {
                            regexp: /^(\w)*$/,
                            message: '密码只能是字母和数字,不能包含空格和其他字符。'
                        }
                    }
                },
                confirmPassword: {
                    validators: {
                        notEmpty: {
                            message: '密码必须填写。'
                        },
                        identical: {
                            field: 'password',
                            message: '两次输入的密码必须完全相同。'
                        },
                        different: {
                            field: 'username',
                            message: '密码不能与用户名相同。'
                        }
                    }
                },
                name: {
                    validators: {
                        notEmpty: {
                            message: '姓名必须填写。'
                        },
                        stringLength: {
                            min: 2,
                            max: 30,
                            message: '姓名长度最少2个字符，最多30个字符。'
                        },
                        regexp: {
                            regexp: /^(\w|[\u4E00-\u9FA5])*$/,
                            message: '姓名只能是中文，字母或者数字,不能包含空格和其他字符。'
                        }
                    }
                },
                email: {
                    validators: {
                        notEmpty: {
                            message: 'Email必须填写！'
                        },
                        emailAddress: {
                            message: '不是有效的email格式，请检查是否有@符号。'
                        }
                    }
                },
                qq: {
                    validators: {
                        notEmpty: {
                            message: 'QQ必须填写！'
                        },
                        stringLength: {
                            min: 4,
                            max: 20,
                            message: 'QQ号码最少4位。'
                        },
                        regexp: {
                            regexp: /^[0-9]+$/,
                            message: 'QQ号码只能是数字。'
                        }
                    }
                },
                phone: {
                    validators: {
                        notEmpty: {
                            message: '号码必须填写！'
                        },
                        regexp: {
                            regexp: /^[0-9-]{6,}$/,
                            message: '电话号码，手机号码只能是数字，最少6位，区号可以用-分隔'
                        }
                    }
                },
                addr: {
                    validators: {
                        notEmpty: {
                            message: '地址必须填写！'
                        },
                        stringLength: {
                            min: 6,
                            max: 50,
                            message: '地址最少6个字符。'
                        },
                    }
                },
                must: {
                    validators: {
                        notEmpty: {
                            message: '必须填写！'
                        },
                    }
                },
            }
        });
    }
}