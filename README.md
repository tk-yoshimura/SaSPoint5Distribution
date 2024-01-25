# AlphaPoint5Distribution
 
This distribution is a special case of a stable distribution with shape parameter &alpha; = 1/2 and skewness parameter &beta; = 0.  
And it does not appear to be widely used.  
*This name is not official but provisional.*

Such a distribution with &beta; = 0 is called symmetric alpha-stable distribution.  
- &alpha; = 2: Normal distribution
- &alpha; = 1: Cauchy distribution

The Alpha point5 distribution, like these distribution, has a closed-from expression, but it can't be expressed in terms of elementary function.  

## Definition
The Alpha point5 distribution, generalized to a stable distribution by introducing position and scale parameters, is as follows:  
![alphahalf1](figures/alphahalf1.svg)  

Since scaling and translations allow for standardization, standard parameters are discussed here.  
![alphahalf2](figures/alphahalf2.svg)  
![alphahalf3](figures/alphahalf3.svg)  

## Numerical Evaluation
Using fresnel integral function, it's obtained as follows:  
This equation becames difficult to obtain accurately when x is extremely small.
![alphahalf4](figures/alphahalf4.svg)  

When x is small, it can be approximated by the following equation:  
![alphahalf5](figures/alphahalf5.svg)  

When x is large, it can be approximated by the following equation:  
![alphahalf6](figures/alphahalf6.svg)  

## Numeric Table
[PDF Precision 150](results/pdf_precision150.csv)  

## Reference
[K.I.Hopcraft, E.Jakeman, R.M.J.Tanner, "Lévy random walks with fluctuating step number and multiscale behavior" (1999)](https://journals.aps.org/pre/abstract/10.1103/PhysRevE.60.5327)  