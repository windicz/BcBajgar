#include "stdafx.h"
#include "stdio.h"
#include "stdlib.h"
#include <cstdlib>
#include <cmath>
#include<mpi.h>
#include<iostream>
#include <time.h>
using namespace std;
int main(int argc, char** argv) {
	int mynode, totalnodes;
	long sum=0, startval, endval, accum,pom;
	float neco = 0;
	double start = 0, stop = 0;
	MPI_Status status;
	MPI_Init(&argc, &argv);

	 MPI_Barrier(MPI_COMM_WORLD);
	 start = MPI_Wtime();

	MPI_Comm_size(MPI_COMM_WORLD, &totalnodes); // get totalnodes
	MPI_Comm_rank(MPI_COMM_WORLD, &mynode); // get myrank
	
	startval = 1000000 * mynode / totalnodes + 1;
	endval = 1000000 * (mynode + 1) / totalnodes;
	for (int i = startval; i <= endval; i = i + 1) {
		for (int l = 0; l <= 1000; l++) {
			sum = sum + i -l;
		}	
	}

	if (mynode != 0)
		MPI_Send(&sum, 1, MPI_INT, 0, 1, MPI_COMM_WORLD);
	else
		for (int j = 1; j < totalnodes; j = j + 1) {
			MPI_Recv(&accum, 1, MPI_INT, j, 1, MPI_COMM_WORLD, &status);
			sum = sum + accum;
		}
	
	if (mynode == 0)
		cout << "The result is: " << sum << endl;

	MPI_Barrier(MPI_COMM_WORLD);
	stop = MPI_Wtime();
	
	MPI_Finalize();


	printf("Elapsed time is %f\n", stop - start);
	return 0;
}

