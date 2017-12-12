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

using Eigen::MatrixXd;
using Eigen::VectorXd;
using Eigen::SparseMatrix;
using Eigen::SparseLU;
using namespace std;

ifstream matrixCs;
ifstream sparseMatrixCs;
ifstream vectorCs;
ifstream matrixSize;

ofstream partialCpp;
ofstream sparseCpp;

ofstream timeCpp;

void ComputeMatrix(int size);
void WriteDoubleFile(VectorXd doubleVector, int size, ofstream &file);
SparseMatrix<double> ReadSparseMatrixFromFileDouble(int size, ifstream &file);
MatrixXd ReadMatrixFromFileDouble(int size, ifstream &file);
VectorXd ReadVectorFromFileDouble(int size, ifstream &file);


void WriteTimeToFile(string time, ofstream &file);

int main(int argc, char** argv)
{
	matrixCs.open("../Output/MatrixCs.txt");
	sparseMatrixCs.open("../Output/SparseMatrixCs.txt");
	vectorCs.open("../Output/VectorCs.txt");
	matrixSize.open("../Output/MatrixSize.txt");
	partialCpp.open("../Output/PartialCpp.txt");
	sparseCpp.open("../Output/SparseCpp.txt");

	timeCpp.open("../Output/TimeCpp.txt");

	int size = 0;
	matrixSize >> size;

	ComputeMatrix(size);

	matrixCs.close();
	sparseMatrixCs.close();
	vectorCs.close();
	matrixSize.close();
	partialCpp.close();
	sparseCpp.close();
	timeCpp.close();

	int x;
	cout << "Program ended its operation" << endl;
	cin >> x;

	return 0;
}

void ComputeMatrix(int size)
{
	MatrixXd matrix = ReadMatrixFromFileDouble(size, matrixCs);
	SparseMatrix<double> sparseMatrix = ReadSparseMatrixFromFileDouble(size, matrixCs);

	SparseLU<SparseMatrix<double>> solver;

	VectorXd vector = ReadVectorFromFileDouble(size, vectorCs);

	auto start = std::chrono::high_resolution_clock::now();
	VectorXd pivLu = matrix.lu().solve(vector);
	auto finish = std::chrono::high_resolution_clock::now();
	double pivLuTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();

	start = std::chrono::high_resolution_clock::now();
	solver.compute(sparseMatrix);
	VectorXd sparseLu = solver.solve(vector);
	finish = std::chrono::high_resolution_clock::now();
	double sparseTime = std::chrono::duration_cast<std::chrono::nanoseconds>(finish - start).count();

	pivLuTime /= 1000000;
	sparseTime /= 1000000;

	string res = to_string(pivLuTime) + " " + to_string(sparseTime);
	WriteTimeToFile(res, timeCpp);

	WriteDoubleFile(pivLu, size, partialCpp);
	WriteDoubleFile(sparseLu, size, sparseCpp);
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

SparseMatrix<double> ReadSparseMatrixFromFileDouble(int size, ifstream &file)
{
	SparseMatrix<double> matrix(size, size);
	string line;
	getline(file, line);
	vector<string> items;
	boost::algorithm::split(items, line, boost::algorithm::is_any_of(" "));
	for (int i = 0; i < size; i++)
	{
		for (int j = 0; j < size; j++)
		{
			int splitNumber = size * i + j;
			matrix.insert(i, j) = atof(items[splitNumber].c_str());
		}
	}
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

void WriteTimeToFile(string time, ofstream &file)
{
	file << time << endl;
}