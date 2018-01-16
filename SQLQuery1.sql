select 
ParentId, COUNT(ChildId)
from ParentChildRel

group by ParentId