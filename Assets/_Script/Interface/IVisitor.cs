using UnityEngine;

public interface IVisitor 
{
    void Visit(IVisitable visitable);
    //void Visit(Health health); //Make relivant components impliment the IVisitable class and write them here
}
public interface IVisitable 
{
    void Accept(IVisitor visitor);
}

///
/// For more information on how the visitor system works see video below
/// For the purposes of this project we will be using the "Intrusive Visitor" pattern 
/// https://www.youtube.com/watch?v=Q2gQs6gIzCM
///