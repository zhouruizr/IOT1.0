using DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataProvider.SqlServer
{
    public class DPButton
    {
        /// <summary>
        /// 获取按钮组
        /// </summary>
        /// <returns></returns>
        public static List<dynamic> DPGetButtonGroup(string BG_Name, string BG_Name_En, int pageSize, int pageIndex, out int allRows, out int allPages)
        {
            using (NERPEntities contex = new NERPEntities())
            {
                int skipCount = ((pageIndex - 1) * pageSize);
                IQueryable<SYS_ButtonGroup> query = contex.SYS_ButtonGroup;
                if (!string.IsNullOrEmpty(BG_Name))
                {
                    query = query.Where(c => c.BG_Name == BG_Name);
                }
                if (!string.IsNullOrEmpty(BG_Name_En))
                {
                    query = query.Where(c => c.BG_Name_En == BG_Name_En);
                }
                //表中有无法转换的字段，所以用动态类型进行转换
                List<dynamic> list = query.Select(c => new { BG_Name = c.BG_Name, BG_Name_En = c.BG_Name_En, BG_OrderIndex = c.BG_OrderIndex, BG_Desc = c.BG_Desc }).ToList<dynamic>();
                //List<SYS_ButtonGroup> list = query.OrderByDescending(c => c.BG_CreatedOn).ToList();
                //foreach (var item in list)
                //{
                //    item.BG_RowVersion = null;
                //}
                allRows = list.Count();
                allPages = allRows / pageSize + allRows % pageSize == 0 ? 0 : 1;
                return pageIndex == 0 ? list : list.Skip(skipCount).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// 获取按钮组
        /// </summary>
        /// <returns></returns>
        public static SYS_ButtonGroup DPGetBtnGroupInformation(string code)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    SYS_ButtonGroup btnGroup = context.SYS_ButtonGroup.Where(c => c.BG_Name_En == code).FirstOrDefault();
                    btnGroup.BG_RowVersion = null;
                    return btnGroup;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
          
        }

        /// <summary>
        /// 验证按钮组属性
        /// </summary>
        /// <returns></returns>
        public static string DPValBtnGroupAttribute(SYS_ButtonGroup bGroup)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    string flag = string.Empty;
                    IQueryable<SYS_ButtonGroup> _valbGroup = context.SYS_ButtonGroup;
                    if (bGroup.BG_Id != 0)
                    {
                        _valbGroup = _valbGroup.Where(c => c.BG_Id != bGroup.BG_Id);
                    }
                    IQueryable<SYS_ButtonGroup> _valName = _valbGroup.Where(c => c.BG_Name == bGroup.BG_Name);
                    if (_valName.Count() != 0)
                    {
                        flag = "0";
                    }
                    else
                    {
                        IQueryable<SYS_ButtonGroup> _valName_En = _valbGroup.Where(c => c.BG_Name_En == bGroup.BG_Name_En);
                        if (_valName_En.Count() != 0)
                        {
                            flag = "1";
                        }
                        else
                        {
                            //_valbGroup = context.SYS_ButtonGroup.Where(c => c.BG_OrderIndex == bGroup.BG_OrderIndex);
                            //if (_valbGroup.Count() != 0)
                            //{
                            //     flag = "2";
                            //}
                            //else
                            //{
                            IQueryable<SYS_ButtonGroup> _valMark = _valbGroup.Where(c => c.BG_Mark == bGroup.BG_Mark);
                            if (_valMark.Count() != 0)
                            {
                                flag = "3";
                            }
                            //}
                        }
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        /// <summary>
        /// 保存按钮组
        /// </summary>
        /// <returns></returns>
        public static void DPSaveButtonGroupModel(SYS_ButtonGroup bGroup)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    bGroup.BG_IsSuspended = false;
                    bGroup.BG_CreatedOn = DateTime.Now;
                    bGroup.BG_CreatedBy = "SYS";
                    context.SYS_ButtonGroup.Add(bGroup);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        /// <summary>
        /// 验证按钮属性
        /// </summary>
        /// <returns></returns>
        public static string DPValBtnAttribute(SYS_Button Btn)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    string flag = string.Empty;
                    IQueryable<SYS_Button> _valbBtn = context.SYS_Button;
                    if (Btn.BTN_Id != 0)
                    {
                        _valbBtn = _valbBtn.Where(c => c.BTN_Id != Btn.BTN_Id);
                    }
                    var _valName = _valbBtn.Where(c => c.BTN_Name == Btn.BTN_Name);
                    if (_valName.Count() != 0)
                    {
                        flag = "0";
                    }
                    else
                    {
                        var _valName_En = _valbBtn.Where(c => c.BTN_Name_En == Btn.BTN_Name_En);
                        if (_valName_En.Count() != 0)
                        {
                            flag = "1";
                        }
                        else
                        {
                            var _valMark = _valbBtn.Where(c => c.BTN_Mark == Btn.BTN_Mark);
                            if (_valMark.Count() != 0)
                            {
                                flag = "3";
                            }
                            //}
                        }
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        /// <summary>
        /// 验证菜单属性
        /// </summary>
        /// <returns></returns>
        public static string DPValMenuAttribute(SYS_FunctionTree Btn)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    string flag = string.Empty;
                    IQueryable<SYS_FunctionTree> _valbBtn = context.SYS_FunctionTree;
                    if (Btn.FT_Id != 0)
                    {
                        _valbBtn = _valbBtn.Where(c => c.FT_Id != Btn.FT_Id);
                    }
                    var _valName = _valbBtn.Where(c => c.FT_Name == Btn.FT_Name);
                    if (_valName.Count() > 0)
                    {
                        flag = "0";
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        /// <summary>
        /// 验证数据字典分类属性--标题和编码不可重复
        /// </summary>
        /// <returns></returns>
        public static string DPValDicCategoryAttribute(SYS_DicCategory Model)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    string flag = string.Empty;
                    IQueryable<SYS_DicCategory> _name = context.SYS_DicCategory;
                    _name = _name.Where(c => c.Title == Model.Title || c.Code == Model.Code);
                    if (Model.DicCategory_Code != "0")
                    {
                        _name = _name.Where(c=>c.DicCategory_Code != Model.DicCategory_Code);
                    }
                    if (_name.Count() > 0)
                    {
                        flag = "0";
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }

        /// <summary>
        /// 验证数据字典属性--标题和编码不可重复
        /// </summary>
        /// <returns></returns>
        public static string DPValDicAttribute(SYS_Dics Model)
        {
            try
            {
                using (NERPEntities context = new NERPEntities())
                {
                    string flag = string.Empty;
                    IQueryable<SYS_Dics> _name = context.SYS_Dics;
                    _name = _name.Where(c => c.Title == Model.Title || c.Code == Model.Code);
                    if (Model.Dics_Code != "0")
                    {
                        if (_name.Count() > 0)
                        {
                            flag = "0";
                        }
                    }
                    return flag;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "转换的过程中发生了错误！");
            }
        }


    }
}
