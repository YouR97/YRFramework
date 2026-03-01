using cfg.Condition;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using YRFramework.Runtime.Manager;

namespace GamePlay.Runtime.Condition
{
    /// <summary>
    /// 条件管理器-解析
    /// </summary>
    public sealed partial class ConditionManager : YRFrameworkManager
    {
        /// <summary>
        /// 条件名解析
        /// </summary>
        private const string CONDITION_REGEX = @"\([^)]*\)";
        /// <summary>
        /// 条件参数解析
        /// </summary>
        private const string CONDITION_ARG_REGEX = @"\((.*?)\)";

        /// <summary>
        /// 复合条件工厂
        /// </summary>
        private IConditionFactor compositeConditionFactor;

        /// <summary>
        /// 根据字符串解析条件
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private ConditionLogicBase ParseExpression(string expr, int conditionExecuterId, ConditionLogicBase parentNode)
        {
            // 处理括号表达式
            if (expr.StartsWith('(') && expr.EndsWith(')'))
                return ParseExpression(expr[1..^1], conditionExecuterId, parentNode);

            // 查找最外层的逻辑运算符
            int depth = 0;
            for (int i = expr.Length - 1; i >= 0; --i)
            {
                char c = expr[i];
                if (')' == c)
                    ++depth;
                else if ('(' == c)
                    --depth;
                else if (0 == depth)
                {
                    if ('|' == c && (0 == i || '|' != expr[i - 1]))
                    {
                        Condition_CompositeLogic orCondition = compositeConditionFactor.Create() as Condition_CompositeLogic;
                        // 处理 OR 运算符
                        ConditionLogicBase left = ParseExpression(expr[..i], conditionExecuterId, orCondition);
                        ConditionLogicBase right = ParseExpression(expr[(i + 1)..], conditionExecuterId, orCondition);
                        orCondition.InitData(E_ConditionOperator.OR, conditionExecuterId, parentNode, left, right);

                        return orCondition;
                    }
                    else if ('&' == c && (0 == i || '&' != expr[i - 1]))
                    {
                        Condition_CompositeLogic andCondition = compositeConditionFactor.Create() as Condition_CompositeLogic;
                        // 处理 AND 运算符
                        ConditionLogicBase left = ParseExpression(expr[..i], conditionExecuterId, andCondition);
                        ConditionLogicBase right = ParseExpression(expr[(i + 1)..], conditionExecuterId, andCondition);
                        andCondition.InitData(E_ConditionOperator.AND, conditionExecuterId, parentNode, left, right);

                        return andCondition;
                    }
                }
            }

            // 没有找到逻辑运算符，是基础条件
            return ParseSingleExpression(expr, conditionExecuterId, parentNode);
        }

        /// <summary>
        /// 单个条件解析
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private ConditionLogicBase ParseSingleExpression(string condition, int conditionExecuterId, ConditionLogicBase parentNode)
        {
            string name = Regex.Replace(condition, CONDITION_REGEX, string.Empty);
            string[] args = null;
            MatchCollection matches = Regex.Matches(condition, CONDITION_ARG_REGEX);
            foreach (Match match in matches.Cast<Match>())
            {
                args = match.Groups[1].Value.Split(',');
            }

            if (!dicConditionType.TryGetValue(name, out E_ConditionType conditionType))
                throw new Exception($"[{nameof(ConditionManager)}]解析条件失败，没有获取到条件类型：{name}");

            if (!dicAllConditionInfo.TryGetValue(conditionType, out ConditionInfo conditionInfo))
                throw new Exception($"[{nameof(ConditionManager)}]解析条件失败，没有获取到条件信息：{conditionType}");

            ConditionLogicBase conditionLogic = conditionInfo.ConditionFactor.Create();
            conditionLogic.InitData(conditionExecuterId, parentNode, args);

            return conditionLogic;
        }
    }
}