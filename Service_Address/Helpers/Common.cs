
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;


namespace Helpers
{
    public class ValidValuesAttribute : ValidationAttribute
    {
        // Danh sách các giá trị hợp lệ
        private string[] _validValues;
        public ValidValuesAttribute(params string[] validValues)
        {
            _validValues = validValues;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null || _validValues.Contains((string)value)) return ValidationResult.Success;
            else
                return new ValidationResult("Invalid value. Just accept");
        }
    }

   

   




    #region MONGO HELPER
    public static class MongoHelper
    {
        /// <summary>
        /// Kiểm tra 1 chuỗi có phải là ObjectID ko
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool isObjectId(this string? input)
        {
            if (ObjectId.TryParse(input, out var id))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Update tất cả các Fields ko null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static UpdateDefinition<T>? ApplyMultiFields<T>(UpdateDefinitionBuilder<T> builder, T? obj)
        {
            if (obj is not null)
            {
                var properties = obj.GetType().GetProperties();
                //--List định nghĩa update
                UpdateDefinition<T>? definition = null;
                //--Lặp quanh danh sách thuộc tính của đối tượng
                foreach (var property in properties)
                {
                    //--lấy giá trị của value
                    var _value = property.GetValue(obj, null);
                    //--Nếu thuộc tính ko null thì thêm vào định nghĩa update
                    if (_value is not null)
                    {
                        definition = (definition is null)
                            //--Nếu definition chưa đc định nghĩa thì đăng ký định nghĩa đầu tiên
                            ? builder.Set(property.Name, _value)
                            //--Sau đó thì chỉ ad trực tiếp vào list định nghĩa
                            : definition = definition.Set(property.Name, _value);
                    }
                }
                return definition;
            }
            else return null;
        }
        /// <summary>
        /// Kiểm tra và thay đổi chuỗi truy vấn update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        //public static string ConvertValueAction<T>(T value)
        //{
        //    switch (value.GetType().Name)
        //    {
        //        case "String": return $"'{value}'";
        //        default: return JsonConvert.SerializeObject(value);
        //    }
        //}
    }
    #endregion
}
