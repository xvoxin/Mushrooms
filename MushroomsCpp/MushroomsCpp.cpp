#include "stdafx.h"
#include <iostream>
#include <sstream>
#include <fstream>
#include <iomanip>
#include <vector>
#include <ctime>
#include <chrono>
#include <windows.h>
#include <math.h> 
#include <boost/algorithm/string/split.hpp>
#include <boost/algorithm/string/classification.hpp>
#include <Eigen/Dense>
#include <Eigen/Sparse>

typedef Eigen::Triplet<double> T;

using Eigen::MatrixXd;
using Eigen::VectorXd;
using Eigen::SparseMatrix;
using Eigen::SparseLU;
using namespace std;

ifstream matrixCs;
ifstream largeMatrixCs;
ifstream vectorCs;

ofstream resCpp;

ofstream timeCpp;

void ComputeMatrixSymetric(int size, int sparseSize);
void WriteDoubleFile(VectorXd doubleVector, int size, ofstream &file);
SparseMatrix<double> ReadSparseMatrixFromFileDouble(int size, int largeSize, ifstream &file);
VectorXd GaussSeidelMethod(MatrixXd matrix, VectorXd vectorB, int size, int numberOfIterations);
double InverseNumber(double x);

MatrixXd ReadMatrixFromFileDouble(int size, ifstream &file);
VectorXd ReadVectorFromFileDouble(int size, ifstream &file);
void WriteDoubleFile(double dobvector[], int size, ofstream &file);
void WriteTimeToFile(string time, ofstream &file);

int main(int argc, char** argv)
{
	matrixCs.open("../Output/MatrixData.txt");
	largeMatrixCs.open("../Output/LargeMatrixData.txt");
	vectorCs.open("../Output/VectorData.txt");
	resCpp.open("../Output/ResCpp.txt");

	timeCpp.open("../Output/TimesCpp.txt");

	int size;

	for (int i = 5; i <= 16; i++)
	{	
		size = (i * 2) * (i * 2) * 2;
		ComputeMatrixSymetric(size, 0);
	}
	
	//ComputeMatrixSymetric(size, 400512);

	matrixCs.close();
	largeMatrixCs.close();
	vectorCs.close();
	resCpp.close();
	timeCpp.close();

	int x;
	cout << "Program ended its operation" << endl;
	cin >> x;

	return 0;
}

void ComputeMatrixSymetric(int size, int sparseSize)
{
	MatrixXd matrix = ReadMatrixFromFileDouble(size, matrixCs);
	SparseMatrix<double> sparseMatrix = ReadSparseMatrixFromFileDouble(size, sparseSize, largeMatrixCs);

	SparseLU<SparseMatrix<double>> solver;

	VectorXd vector = ReadVectorFromFileDouble(size, vectorCs);
	
	auto start = std::chrono::high_resolution_clock::now();
	VectorXd seidel = GaussSeidelMethod(matrix, vector, size, 7000);
	auto finish = std::chrono::high_resolution_clock::now();
	double seidelTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();

	solver.analyzePattern(sparseMatrix);
	solver.factorize(sparseMatrix);	
	solver.compute(sparseMatrix);

	start = std::chrono::high_resolution_clock::now();
	VectorXd sparseLu = solver.solve(vector);
	finish = std::chrono::high_resolution_clock::now();
	double sparseTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();

	sparseTime /= 1000000;
	seidelTime /= 1000000;

	string res = to_string(seidelTime) + " " + to_string(sparseTime);
	WriteTimeToFile(res, timeCpp);

	double output[] = { seidel(0), sparse(0) };
	WriteDoubleFile(output, 2, resCpp);
}

VectorXd GaussSeidelMethod(MatrixXd matrix, VectorXd vectorB, int size, int numberOfIterations)
{
	MatrixXd U(size, size);
	MatrixXd D(size, size);
	MatrixXd L(size, size);
	VectorXd x1(size);
	VectorXd x2(size);

	double x1norm = 0;
	double x2norm = 0;

	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			if (i == j)
			{
				D(i, j) = InverseNumber(matrix(i, j));
			}
			else if (i > j)
			{
				L(i, j) = matrix(i, j);
			}
			else if (i < j)
			{
				U(i, j) = matrix(i, j);
			}
		}
	}
	for (int k = 0; k < numberOfIterations; k++)
	{
		for (int i = 0; i < size; i++)
		{
			x1(i) = vectorB(i) * D(i, i);
			for (int j = 0; j < i; j++)
			{
				x1(i) -= D(i, i) * L(i, j) * x1(j);
			}
			for (int j = i + 1; j < size; j++)
			{
				x1(i) -= D(i, i) * U(i, j) * x1(j);
			}
		}
		for (int j = 0; j < size; j++)
		{
			x1norm += x1(j) * x1(j);
			x2norm += x2(j) * x2(j);
		}

		x2norm = sqrt(x2norm);
		x1norm = sqrt(x1norm);

		if (abs(x1norm) - abs(x2norm) < 1e-10 && x2(0) > 0)
		{
			cout << "Gauss Seidel breaks on iteration nr - " << k << endl;
			break;
		}

		for (int j = 0; j < size; j++)
		{
			x2(j) = x1(j);
		}
	}
	return x1;
}

double InverseNumber(double x)
{
	if (abs(x) < 1e-14)
		return 0;

	return 1 / x;
}

MatrixXd ReadMatrixFromFileDouble(int size, ifstream &file)
{
	MatrixXd matrix(size, size);
	string line;
	getline(file, line);
	vector<string> items;
	boost::algorithm::split(items, line, boost::algorithm::is_any_of(" "));
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			int splitNumber = size * i + j;
			matrix(i, j) = atof(items[splitNumber].c_str());
		}
	}
	return matrix;
}

SparseMatrix<double> ReadSparseMatrixFromFileDouble(int size, int largeSize, ifstream &file)
{
	SparseMatrix<double> matrix(size, size);
	string line;
	getline(file, line);
	vector<string> items;
	boost::algorithm::split(items, line, boost::algorithm::is_any_of(" "));

	for (int i = 0; i < largeSize * 3; i += 3)
	{
		int x = (int)atof(items[i].c_str());
		int y = (int)atof(items[i + 1].c_str());
		double value = atof(items[i + 2].c_str());
		matrix.insert(x, y) = value;
	}
	matrix.makeCompressed();
	return matrix;
}

VectorXd ReadVectorFromFileDouble(int size, ifstream &file)
{
	VectorXd vec(size);
	string line;
	getline(file, line);
	vector<string> items;
	boost::algorithm::split(items, line, boost::algorithm::is_any_of(" "));


	for (int i = 0; i < size; i++)
	{
		vec(i) = atof(items[i].c_str());
	}
	return vec;
}

void WriteDoubleFile(VectorXd doubleVector, int size, ofstream &file)
{
	for (int i = 0; i < size; i++)
	{
		file << setprecision(16) << doubleVector(i);
		file << " ";
	}
	file << "\n";
}

void WriteDoubleFile(double dobvector[], int size, ofstream &file) 
{
	for (int i = 0; i < size; i++)
	{
		file << setprecision(16) << dobvector[i];
		file << " ";
	}
	file << "\n";
}

void WriteTimeToFile(string time, ofstream &file)
{
	file << time << endl;
}