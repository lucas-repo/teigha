module Ast

type expr = 
  | Literal of string 
  | Float of float
  | Vector of expr*expr*expr
  | Table of int64 * string
  | TableEval of int64 * string * string * string
  | ImplicitTableEval of string * string * string
  | Sin of expr
  | Cos of expr
  | Tan of expr
  | ASin of expr
  | ACos of expr
  | ATan of expr
  | Plus of expr * expr
  | Minus of expr * expr
  | Div of expr * expr
  | Mul of expr * expr
