using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Assets.Code.Utils
{


    public class CoordinateConverter
    {
        private double[,] matrizConversion;
        private float defaultAltitude = 1f;

        //Datos de las ubicaciones ORIGINALES
        //https://sabuda.grc.upv.es/api/applications/5f8edc99a3302d002d06241d/UserHardSensorslocations/634d862df29d2d0020948ed3
        //AlbujonDesembocadura (Photrack Site: 2346)        ///Cubo A       (37.71635, -0.86091, 0)
        //dkMarchamalo KA-079188 (Photrack Site: 2202)      ///Cubo B       (37.65387, -0.71971, 0)
        //dkEstacio KA-079188 (Photrack Site: 2203)         ///Cubo C       (37.74698,-0.73319,0)
        /// those coordinates are mapped with execution Time Position Transfomate of CubeA, CubeB, CubeC from map.

        public CoordinateConverter(Vector3 q1, Vector3 q2, Vector3 q3, float defaultAltitude) ///Coordenadas represantivas de Unity
        {
            this.defaultAltitude = defaultAltitude;
            if (matrizConversion == null)
            {


                // Puntos de referencia en el sistema original
                Vector3 p1 = new Vector3(37.71635f, -0.86091f, defaultAltitude);
                Vector3 p2 = new Vector3(37.65387f, -0.71971f, defaultAltitude);
                Vector3 p3 = new Vector3(37.74698f, -0.73319f, defaultAltitude);


                double[,] A = new double[3, 3] {
                                        { p1.x, p2.x, p3.x },
                                        { p1.y, p2.y, p3.y },
                                        { p1.z, p2.z, p3.z } };
                double[,] B = new double[3, 3] {
                                        { q1.x, q2.x, q3.x },
                                        { q1.y, q2.y, q3.y },
                                        { q1.z, q2.z, q3.z } };

                double[,] inverse = this.InverseMatrix(A);
                matrizConversion = this.MultiplicarMatricesGenerico(B, inverse); //B*inverse


                //Debug.Log("Matriz A: "+A);
                //Debug.Log("Matriz Inversa: "+inverse);
                //Debug.Log("Matriz de conversión: "+ matrizConversion);
            }
        }

        public void EjemploUso()
        {
            //37.720819, -0.862193,1 = (5.97, 3, 2.88)
            //(37.720819, -0.862193) = (5.97, 2.88)
            double lon = 37.720819f;
            double lat = -0.862193f;
            double alt = defaultAltitude;
            double[] p1 = new double[3] { lon, lat, alt }; 
            Vector3 q1 = this.TransformarCoordenadas(p1);
            Debug.Log("Coordenadas originales: " + $"[{p1[0]},{p1[1]}]");
            Debug.Log("Coordenadas transformadas: " + q1);
            Debug.Log("//37.720819, -0.862193,1 = (5.97, 3, 2.88)");
        }


        public Vector3 TransformarCoordenadas(double[] latLon)
        {
            /// TODO: Reajusta un delta que ocupe la altura! 
            double[,] coordenadasAntiguas = new double[3, 1] { { latLon[0] }, { latLon[1] }, { defaultAltitude } };
            //double[,] coordenadasAntiguas = new double[3, 1] { { latLon[0] }, { latLon[1] }, { latLon[2] } };
            double[,] result = MultiplicarMatricesGenerico(matrizConversion, coordenadasAntiguas);
            Vector3 coordernadasFinales = new Vector3();
            coordernadasFinales.x = (float)result[0, 0];
            //coordernadasFinales.y = (float)result[1, 0]/deltaAltura;
            coordernadasFinales.y = defaultAltitude;
            coordernadasFinales.z = (float)result[2, 0];




            //Debug.Log("***Matriz resultado: " + String.Format("[{0}||{1}||{2}] xd", result[0,0], result[1,0], result[2,0]));
            //Debug.Log("Vector3: " + coordernadasFinales);
            return coordernadasFinales;
        }

        #region Metodos privados de apoyo para el converter

        private double[,] MultiplicarMatricesGenerico(double[,] matrix1, double[,] matrix2)
        {
            int rows1 = matrix1.GetLength(0);
            int cols1 = matrix1.GetLength(1);
            int rows2 = matrix2.GetLength(0);
            int cols2 = matrix2.GetLength(1);

            if (cols1 != rows2)
            {
                Debug.Log("Las matrices no son compatibles para multiplicar");
                throw new ArgumentException("Las matrices no son compatibles para multiplicar");
            }

            double[,] result = new double[rows1, cols2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < cols1; k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        private double[,] InverseMatrix(double[,] matrix)
        {
            int order = matrix.GetLength(0);
            double[,] result = new double[order, order];

            // Calculate the determinant of the matrix
            double det = CalculateDeterminant(matrix);

            if (det == 0)
            {
                throw new Exception("Matrix is not invertible.");
            }

            // Calculate the adjugate matrix
            double[,] adj = new double[order, order];
            for (int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    adj[i, j] = Math.Pow(-1, i + j) * CalculateDeterminant(GetSubmatrix(matrix, i, j)) / det;
                }
            }

            // Transpose the adjugate matrix to get the inverse matrix
            for (int i = 0; i < order; i++)
            {
                for (int j = 0; j < order; j++)
                {
                    result[i, j] = adj[j, i];
                }
            }

            return result;
        }

        private static double CalculateDeterminant(double[,] matrix)
        {
            int order = matrix.GetLength(0);

            if (order == 2)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }

            double det = 0;
            for (int i = 0; i < order; i++)
            {
                det += Math.Pow(-1, i) * matrix[0, i] * CalculateDeterminant(GetSubmatrix(matrix, 0, i));
            }

            return det;
        }

        private static double[,] GetSubmatrix(double[,] matrix, int rowToRemove, int columnToRemove)
        {
            int order = matrix.GetLength(0);
            double[,] result = new double[order - 1, order - 1];

            int iResult = 0;
            for (int i = 0; i < order; i++)
            {
                if (i == rowToRemove)
                {
                    continue;
                }

                int jResult = 0;
                for (int j = 0; j < order; j++)
                {
                    if (j == columnToRemove)
                    {
                        continue;
                    }

                    result[iResult, jResult] = matrix[i, j];
                    jResult++;
                }

                iResult++;
            }

            return result;
        }
        #endregion





    }
}
