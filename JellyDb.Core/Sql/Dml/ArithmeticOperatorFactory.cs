//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using JellyDb.Sql.Dml;

//namespace JellyDb.Core.Sql.Dml
//{
//    public class ArithmeticOperatorFactory
//    {
//        public dynamic Create(Term leftOperand, Term rightOperand, ArithmeticOperationTypeEnum operationType)
//        {
//            if (leftOperand is FloatValueTerm || rightOperand is FloatValueTerm)
//                return BuildFloatOperator(operationType);

//            return BuildIntOperator(operationType);
//        }

        
//        private dynamic BuildFloatOperator(ArithmeticOperationTypeEnum operationType)
//        {
//            switch (operationType)
//            {
//                case ArithmeticOperationTypeEnum.Addition:
//                    return new AdditionOperator<float>();
//                case ArithmeticOperationTypeEnum.Subtraction:
//                    return new SubtractionOperator<float>();
//                case ArithmeticOperationTypeEnum.Multiplication:
//                    return new MultiplicationOperator<float>();
//                case ArithmeticOperationTypeEnum.Division:
//                    return new DivisionOperator<float>();
//                case ArithmeticOperationTypeEnum.Modulo:
//                    return new ModuloOperator<float>(); 
//                default:
//                    throw new InvalidOperationException("Invalid type of ArithmeticOperation. Operator does not exist.");
//            }
//        }

//        private dynamic BuildIntOperator(ArithmeticOperationTypeEnum operationType)
//        {
//            switch (operationType)
//            {
//                case ArithmeticOperationTypeEnum.Addition:
//                    return new AdditionOperator<int>();
//                case ArithmeticOperationTypeEnum.Subtraction:
//                    return new SubtractionOperator<int>();
//                case ArithmeticOperationTypeEnum.Multiplication:
//                    return new MultiplicationOperator<int>();
//                case ArithmeticOperationTypeEnum.Division:
//                    return new DivisionOperator<int>();
//                case ArithmeticOperationTypeEnum.Modulo:
//                    return new ModuloOperator<int>();
//                default:
//                    throw new InvalidOperationException("Invalid type of ArithmeticOperation. Operator does not exist.");
//            }
//        }
            
//    }
//}
